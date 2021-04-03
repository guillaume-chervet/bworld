using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Data.Tags.Models;
using Demo.Mvc.Core.Tags.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Tags
{
    public class SaveTagsCommand : Command<UserInput<SaveTagsInput>, CommandResult<dynamic>>
    {
        private readonly UserService _userService;
        private readonly TagsServiceMongo _tagsServiceMongo;

        public SaveTagsCommand(UserService userService, TagsServiceMongo tagsServiceMongo)
        {
            _userService = userService;
            _tagsServiceMongo = tagsServiceMongo;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var tagsDbModel = await _tagsServiceMongo.FindTags(siteId, Input.Data.Type);

            if (tagsDbModel == null)
            {
                tagsDbModel = new TagsDbModel()
                {
                    SiteId = siteId,
                    Type = Input.Data.Type,
                    ModuleId = Input.Data.ModuleId,
                    Tags = new List<TagDbModel>()
                };
            }

            foreach (var tag in Input.Data.Tags)
            {
                TagDbModel tagDbModel = null;
                if (!string.IsNullOrEmpty(tag.Id))
                {
                    tagDbModel = tagsDbModel.Tags.FirstOrDefault(t => t.Id == tag.Id);
                }
                if(tagDbModel == null)
                {
                    tagDbModel = new TagDbModel();
                    tagsDbModel.Tags.Add(tagDbModel);
                }

                tagDbModel.Name = tag.Name;
                tagDbModel.IsDeleted = tag.IsDeleted;

            }
            await _tagsServiceMongo.SaveAsync(tagsDbModel);

        }
    }
}