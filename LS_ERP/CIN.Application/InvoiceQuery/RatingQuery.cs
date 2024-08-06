using AutoMapper;
using CIN.Application.Common;
using CIN.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{
    #region GetSelectRatingList

    public class GetSelectRatingList : IRequest<List<CustomSelectListItem>>
    {
    }

    public class GetSelectRatingListHandler : IRequestHandler<GetSelectRatingList, List<CustomSelectListItem>>
    {
        public GetSelectRatingListHandler()
        {
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectRatingList request, CancellationToken cancellationToken)
        {
            return await RatingItemList();
        }

        private Task<List<CustomSelectListItem>> RatingItemList() => Task.FromResult(new List<CustomSelectListItem>
        {
             new(){ Text = "None", TextTwo = "لا يوجد", Value="0" },
             new(){ Text = "Bad", TextTwo = "سيء", Value="1" },
             new(){ Text = "Average", TextTwo = "متوسط", Value="2" },
             new(){ Text = "Good", TextTwo = "جيد", Value="3" },
             new(){ Text = "Excellent", TextTwo = "ممتاز", Value="4" },
             new(){ Text = "The Best / The Most Outstanding", TextTwo = "الافضل / متفوق", Value="5" },
        });
    }

    #endregion
}
