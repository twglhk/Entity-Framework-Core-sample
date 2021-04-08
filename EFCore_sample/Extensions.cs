using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_sample
{
    public static class Extensions
    {
        public static IQueryable<GuildDto> MapGuildToDto(this IQueryable<Guild> guild)
        {
            return guild.Select(g => new GuildDto
            {
                Name = g.GuildName,
                MemberCount = g.Members.Count
            });
        }
    }
}
