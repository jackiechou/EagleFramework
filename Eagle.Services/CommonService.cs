using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.Common;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services
{
    public class CommonService: BaseService, ICommonService
    {
        public CommonService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public SelectList GetMonths(Guid applicationId, string selectedValue, bool? isShowSelectText = false)
        {
            var language = UnitOfWork.ApplicationLanguageRepository.GetSelectedLanguage(applicationId);
            return UnitOfWork.CommonRepository.GetMonths(language.LanguageCode, selectedValue, isShowSelectText);
        }

        public SelectList GetYears(int numberOfYears, string selectedValue, bool? isShowSelectText = false)
        {
            return UnitOfWork.CommonRepository.GetYears(numberOfYears, selectedValue, isShowSelectText);
        }

        public SelectList GenerateThreeStatusModeList(string selectedValue, bool? isShowSelectText)
        {
            return UnitOfWork.CommonRepository.GenerateThreeStatusModeList(LanguageResource.All, true);
        }

        #region HIEARACHY ====================================================================================================

        public IEnumerable<TreeNodeDetail> RecursiveFillTreeNodes(IEnumerable<TreeNodeDetail> elements, int? parentId=0)
        {
            if (elements == null) return null;
            List<TreeNodeDetail> items = new List<TreeNodeDetail>();
            List<TreeNodeDetail> children = elements.Where(p => p.ParentId == parentId).Select(
               p => new TreeNodeDetail
               {
                   Id = p.Id,
                   Text = p.Text,
                   ParentId = p.ParentId,
                   Depth = p.Depth
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeNodeDetail()
                {
                    Id = child.Id, ParentId = child.ParentId, Text = child.Text, Depth = child.Depth, Children = RecursiveFillTreeNodes(elements, child.Id)
                }));
            }
            return items;
        }

        public List<TreeDetail> RecursiveFillTree(IEnumerable<TreeDetail> elements, int? parentid)
        {
            if (elements == null) return null;
            List<TreeDetail> items = new List<TreeDetail>();
            List<TreeDetail> children = elements.Where(p => p.parentid == (parentid ?? 0)).Select(
               p => new TreeDetail
               {
                   id = p.id,
                   key = p.key,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   parentid = p.parentid,
                   depth = p.depth,
                   hasChild = p.hasChild,
                   folder = (p.hasChild != null && p.hasChild == true),
                   lazy = (p.hasChild != null && p.hasChild == true),
                   expanded = (p.hasChild != null && p.hasChild == true),
                   tooltip = p.tooltip
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeDetail()
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    depth = child.depth,
                    folder = (child.hasChild != null && child.hasChild == true),
                    lazy = (child.hasChild != null && child.hasChild == true),
                    expanded = (child.hasChild != null && child.hasChild ==true),
                    tooltip = child.tooltip,
                    children = RecursiveFillTree(elements, child.key)
                }));
            }
            return items;
        }

        public List<TreeDetail> RecursiveFillTree(IEnumerable<TreeDetail> elements, int? parentid, int? selectedId)
        {
            if (elements == null) return null;
            List<TreeDetail> items = new List<TreeDetail>();
            List<TreeDetail> children = elements.Where(p => p.parentid == (parentid ?? 0)).Select(
               p => new TreeDetail
               {
                   id = p.id,
                   key = p.key,
                   name =p.name,
                   title = p.title,
                   text = p.text,
                   parentid = p.parentid,
                   depth = p.depth,
                   hasChild = p.hasChild,
                   folder = (p.hasChild != null && p.hasChild == true),
                   lazy = (p.hasChild != null && p.hasChild == true),
                   expanded = (p.hasChild != null && p.hasChild == true),
                   selected = (selectedId != null && p.id == selectedId),
                   tooltip = p.tooltip
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeDetail()
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    depth = child.depth,
                    folder = (child.hasChild != null && child.hasChild == true),
                    lazy = (child.hasChild != null && child.hasChild == true),
                    expanded = (child.hasChild != null && child.hasChild == true),
                    selected = (selectedId != null && child.id == selectedId),
                    tooltip = child.tooltip,
                    children = RecursiveFillTree(elements, child.key, selectedId)
                }));
            }
            return items;
        }
        #endregion ===========================================================================================================

    }
}
