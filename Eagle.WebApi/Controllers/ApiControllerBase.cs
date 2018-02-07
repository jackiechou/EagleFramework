using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using Eagle.Core.Common;
using Eagle.Core.Pagination;
using Eagle.Services;
using Eagle.Services.Dtos;
using Eagle.Services.Dtos.Common;
using Eagle.WebApi.Results;
using Newtonsoft.Json;

namespace Eagle.WebApi.Controllers
{
    /// <summary>
    /// Base Api Controller - All APIs are authorized to an authenticated user within the restricted IPs and/or IP Ranges be default.
    /// </summary>
    //[ServiceAuthorization(AllowAnonymous = false)]
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        protected ApiControllerBase(IEnumerable<IBaseService> services)
        {
            DisposableManager = new DisposableManager();

            Services = services;
        }

        /// <summary>
        /// Sets the identity.
        /// </summary>
        internal void SetIdentity(ClaimsPrincipal identity)
        {
            CurrentClaimsIdentity = identity;
            foreach (var service in Services)
            {
                service.SetIdentity(identity);
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        protected IEnumerable<IBaseService> Services { get; private set; }

        /// <summary>
        /// Gets the current claims identity.
        /// </summary>
        /// <value>
        /// The current claims identity.
        /// </value>
        internal protected ClaimsPrincipal CurrentClaimsIdentity { get; private set; }

        /// <summary>
        /// List of Disposable Objects
        /// </summary>
        protected IList<IDisposable> Disposables
        {
            get { return DisposableManager.Disposables; }
        }

        private IDisposableManager DisposableManager { get; set; }



        /// <summary>
        /// Resolves required object
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        protected TObject Resolve<TObject>()
            where TObject : class
        {
            var result = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(TObject)) as TObject;
            var disposable = result as IDisposable;
            if (disposable != null)
            {
                Disposables.Add(disposable);
            }
            return result;
        }

        #region RESULT METHODS
        /// <summary>
        /// Creates the ok result.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="includeTemporaryAccessToken"></param>
        /// <returns></returns>
        protected OkWithHeaders<IEnumerable<TData>> CreateOkResult<TData>(IEnumerable<TData> data, int? pageSize = null, int? page = null, int? totalCount = null, bool includeTemporaryAccessKey = false)
        {

            var pageCount = pageSize != null && totalCount != null ? (totalCount / pageSize) + (totalCount % pageSize > 0 ? 1 : 0) : 0;

            var paginationHeader = new
            {
                TotalCount = totalCount ?? 0,
                TotalPages = pageCount,
                PageSize = pageSize,
                Page = page
            };

            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("X-Pagination", JsonConvert.SerializeObject(paginationHeader))
            };

            //if (includeTemporaryAccessKey)
            //{
            //    headers.Add(new KeyValuePair<string, string>("X-TAK", BaseSecurityService.SetTemporaryAccessToken()));
            //}

            var result = new OkWithHeaders<IEnumerable<TData>>(data, this, headers);

            return result;
        }

        /// <summary>
        /// Creates the ok result.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <typeparam name="TItemResult">The type of the item result.</typeparam>
        /// <typeparam name="TListResult">The type of the list result.</typeparam>
        /// <param name="listResult">The list result.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <param name="totalCount">The total count.</param>
        /// <returns></returns>
        protected OkWithHeaders<TListResult> CreateOkResult<TData, TItemResult, TListResult>(TListResult listResult, int? pageSize = null, int? page = null, int? totalCount = null)
            where TData : DtoBase
            where TItemResult : ItemResult<TData>, new()
            where TListResult : ListResult<TData, TItemResult>, new()
        {
            var pageCount = pageSize != null && totalCount != null ? (totalCount / pageSize) + (totalCount % pageSize > 0 ? 1 : 0) : 0;

            var paginationHeader = new
            {
                TotalCount = totalCount ?? 0,
                TotalPages = pageCount,
                PageSize = pageSize,
                Page = page
            };

            //TODO: Remove header after UI team change front-end
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("X-Pagination", JsonConvert.SerializeObject(paginationHeader))
            };

            listResult.Pagination = new Pagination { Page = page, PageSize = pageSize, TotalCount = totalCount ?? 0, TotalPages = pageCount.Value };

            var result = new OkWithHeaders<TListResult>(listResult, this, headers);

            return result;
        }

        /// <summary>
        /// Creates the ok result.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="itemResult">The item result.</param>
        /// <returns></returns>
        protected OkWithHeaders<ItemResult<TData>> CreateOkResult<TData>(ItemResult<TData> itemResult)
            where TData : DtoBase
        {
            var result = new OkWithHeaders<ItemResult<TData>>(itemResult, this, new List<KeyValuePair<string, string>>());

            return result;
        }

        /// <summary>
        /// Creates Ok or NotFound result for Get verb
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TItemResult"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected IHttpActionResult CreateGetResult<TDto, TItemResult>(TDto dto)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
        {
            return (dto != null) ? (IHttpActionResult)Ok(dto.ToItemResult<TDto, TItemResult>()) : NotFound();
        }

        /// <summary>
        ///  Ok or NotFound result for Get verb
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TItemResult"></typeparam>
        /// <param name="dto"></param>
        /// <param name="getDtoActions"></param>
        /// <returns></returns>
        protected IHttpActionResult CreateGetResult<TDto, TItemResult>(TDto dto,
            Func<TDto, IEnumerable<EntityAction>> getDtoActions = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
        {
            return (dto != null) ? (IHttpActionResult)Ok(dto.ToItemResult<TDto, TItemResult>(null, getDtoActions)) : NotFound();
        }

        /// <summary>
        /// Creates Ok or NotFound result for Get verb
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TItemResult"></typeparam>
        /// <typeparam name="TListResult"></typeparam>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected IHttpActionResult CreateGetResult<TDto, TItemResult, TListResult>(IEnumerable<TDto> dtos)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
            where TListResult : ListResult<TDto, TItemResult>, new()
        {
            return (dtos != null) ? (IHttpActionResult)Ok(dtos.ToListResult<TDto, TItemResult, TListResult>()) : NotFound();
        }

        /// <summary>
        /// Creates Ok or NotFound result for Get verb
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TItemResult"></typeparam>
        /// <typeparam name="TListResult"></typeparam>
        /// <param name="dtos"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        protected IHttpActionResult CreateGetResult<TDto, TItemResult, TListResult>(IEnumerable<TDto> dtos, int? pageSize = null, int? page = null, int? totalCount = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
            where TListResult : ListResult<TDto, TItemResult>, new()
        {
            return (dtos != null) ? (IHttpActionResult)CreateOkResult<TDto, TItemResult, TListResult>(dtos.ToListResult<TDto, TItemResult, TListResult>(), pageSize, page, totalCount) : NotFound();
        }

        /// <summary>
        /// Creates Ok or NotFound result for Get verb
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TItemResult"></typeparam>
        /// <typeparam name="TListResult"></typeparam>
        /// <param name="dtos"></param>
        /// <param name="getDtoActions"></param>
        /// <param name="getListActions"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        protected IHttpActionResult CreateGetResult<TDto, TItemResult, TListResult>(IEnumerable<TDto> dtos,
            Func<TDto, IEnumerable<EntityAction>> getDtoActions = null,
            Func<IEnumerable<TItemResult>, IEnumerable<EntityAction>> getListActions = null,
            int? pageSize = null, int? page = null, int? totalCount = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
            where TListResult : ListResult<TDto, TItemResult>, new()
        {
            return (dtos != null) ? (IHttpActionResult)CreateOkResult<TDto, TItemResult, TListResult>(
                dtos.ToListResult<TDto, TItemResult, TListResult>(getDtoActions, getListActions),
                pageSize, page, totalCount) : NotFound();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DisposableManager != null)
                    DisposableManager.Dispose(disposing);

                DisposableManager = null;
                Services = null;
            }
            base.Dispose(disposing);

        }
    }
}
