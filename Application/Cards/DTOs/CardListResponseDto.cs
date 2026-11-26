namespace Application.Cards.DTOs;

public sealed record CardListItemDto(
	long Id,
	string Name,
	string? Type,
	string? ImageUrlSmall,
	string? SetRarity,
	decimal? SetPrice);

public sealed record PaginationDto(int Page, int Size, int Total);

public sealed record CardSearchSuggestionDto(
	string OriginalQuery,
	string SuggestedQuery);

public sealed record CardListResponseDto(
	IReadOnlyList<CardListItemDto> Items,
	PaginationDto Pagination,
	CardSearchSuggestionDto? Suggestion);
