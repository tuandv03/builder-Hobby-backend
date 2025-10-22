using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cards.Queries
{
	public record GetCardByIdQuery(int Id) : IRequest<Card>;

	public record GetCardDetailByIdQuery(int Id) : IRequest<Card>;
}
