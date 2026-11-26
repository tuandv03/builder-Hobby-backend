using System.Text;

namespace Infrastructure.Search;

internal static class CardNameMatcher
{
	public static string? FindClosest(string query, IEnumerable<string> candidates)
	{
		var normalizedQuery = Normalize(query);
		if (normalizedQuery.Length < 2)
		{
			return null;
		}

		string? bestCandidate = null;
		var bestDistance = int.MaxValue;
		var bestSimilarity = 0d;

		foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
		{
			var normalizedCandidate = Normalize(candidate);
			if (normalizedCandidate.Length == 0)
			{
				continue;
			}

			var distance = DamerauLevenshtein(normalizedQuery, normalizedCandidate);
			var similarity = 1d - (double)distance / Math.Max(normalizedQuery.Length, normalizedCandidate.Length);

			if (similarity > bestSimilarity ||
				(Math.Abs(similarity - bestSimilarity) < 0.0001 && distance < bestDistance))
			{
				bestCandidate = candidate;
				bestDistance = distance;
				bestSimilarity = similarity;
			}
		}

		var maximumDistance = normalizedQuery.Length switch
		{
			<= 4 => 1,
			<= 8 => 2,
			_ => 3
		};

		return bestDistance <= maximumDistance && bestSimilarity >= 0.70
			? bestCandidate
			: null;
	}

	internal static string Normalize(string value)
	{
		var output = new StringBuilder(value.Length);
		var previousWasSpace = true;

		foreach (var character in value.ToLowerInvariant())
		{
			if (char.IsAsciiLetterOrDigit(character))
			{
				output.Append(character);
				previousWasSpace = false;
			}
			else if (!previousWasSpace)
			{
				output.Append(' ');
				previousWasSpace = true;
			}
		}

		return output.ToString().Trim();
	}

	internal static int DamerauLevenshtein(string source, string target)
	{
		var distances = new int[source.Length + 1, target.Length + 1];

		for (var i = 0; i <= source.Length; i++) distances[i, 0] = i;
		for (var j = 0; j <= target.Length; j++) distances[0, j] = j;

		for (var i = 1; i <= source.Length; i++)
		{
			for (var j = 1; j <= target.Length; j++)
			{
				var substitutionCost = source[i - 1] == target[j - 1] ? 0 : 1;
				distances[i, j] = Math.Min(
					Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
					distances[i - 1, j - 1] + substitutionCost);

				if (i > 1 && j > 1 &&
					source[i - 1] == target[j - 2] &&
					source[i - 2] == target[j - 1])
				{
					distances[i, j] = Math.Min(distances[i, j], distances[i - 2, j - 2] + 1);
				}
			}
		}

		return distances[source.Length, target.Length];
	}
}
