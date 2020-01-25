using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data.Tags;

namespace Demo.Business.Command.Tags
{
    public class GetTagsCommand : Command<UserInput<GetTagsInput>, CommandResult<GetTagsResult>>
    {
        private readonly TagsServiceMongo _tagsServiceMongo;

        public GetTagsCommand( TagsServiceMongo tagsServiceMongo)
        {
            _tagsServiceMongo = tagsServiceMongo;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;

            var tagsDbModel = await _tagsServiceMongo.FindTags(siteId, Input.Data.Type);

            IList<Tag> tags = new List<Tag>();

            if (tagsDbModel != null && tagsDbModel.Tags != null)
            {
                foreach (var tagDbModel in tagsDbModel.Tags)
                {
                    tags.Add(new Tag()
                    {
                        Id = tagDbModel.Id,
                        Name = tagDbModel.Name,
                        IsDeleted = tagDbModel.IsDeleted
                    });
                }
            }

            Result.Data = new GetTagsResult()
            {
                Tags = tags//new List<Tag>() {new Tag() {Id = "1", Name = "Client"}, new Tag() {Id = "2", Name = "Prospet"}}
            };
        }
    }
}