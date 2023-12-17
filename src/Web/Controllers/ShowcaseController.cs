using Core.Entities.ShowcaseAggregate;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Linq.Expressions;
using Web.BindingModels;

namespace Web.Controllers
{
    public class ShowcaseController : BaseAdminController
    {
        public ShowcaseController(
            CatalogContext context,
            ILocalStorageService service) : base(context, service) { }

        public async Task<IActionResult> Edit([FromForm] EditShowcaseCommand cmd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ShowcaseTemplate template;

            try
            {
                template = await _context.Showcases
                        .Include(s => s.Main)
                        .Include(s => s.Mone)
                        .Include(s => s.Mtwo)
                        .Include(s => s.Sone)
                        .Include(s => s.Stwo)
                        .Include(s => s.Sthree)
                        .Include(s => s.Sfour).OrderBy(s => s.Id).LastAsync();
            }
            catch (Exception ex)
            {
                await Init();

                template = await _context.Showcases
                        .Include(s => s.Main)
                        .Include(s => s.Mone)
                        .Include(s => s.Mtwo)
                        .Include(s => s.Sone)
                        .Include(s => s.Stwo)
                        .Include(s => s.Sthree)
                        .Include(s => s.Sfour).OrderBy(s => s.Id).LastAsync();
            }
            
            template.Main.Title = cmd.Titles[0];
            template.Main.Subtitle = cmd.Subtitles[0];
            template.Main.Link = cmd.Links[0];
            if (cmd.Image1 != null)
            {
                template.Main.Image = await _storage.SaveFile(cmd.Image1, "showcases");
            }
            template.Mone.Title = cmd.Titles[1];
            template.Mone.Subtitle = cmd.Subtitles[1];
            template.Mone.Link = cmd.Links[1];
            if (cmd.Image2 != null)
            {
                template.Mone.Image = await _storage.SaveFile(cmd.Image2, "showcases");
            }
            template.Mtwo.Title = cmd.Titles[2];
            template.Mtwo.Subtitle = cmd.Subtitles[2];
            template.Mtwo.Link = cmd.Links[2];
            if (cmd.Image3 != null)
            {
                template.Mtwo.Image = await _storage.SaveFile(cmd.Image3, "showcases");
            }
            template.Sone.Title = cmd.CTitles[0];
            template.Sone.Subtitle = cmd.CSubtitles[0];
            template.Sone.Link = cmd.CLinks[0];
            if (cmd.CImage1 != null)
            {
                template.Sone.Image = await _storage.SaveFile(cmd.CImage1, "showcases");
            }
            template.Stwo.Title = cmd.CTitles[1];
            template.Stwo.Subtitle = cmd.CSubtitles[1];
            template.Stwo.Link = cmd.CLinks[1];
            if (cmd.CImage2 != null)
            {
                template.Stwo.Image = await _storage.SaveFile(cmd.CImage2, "showcases");
            }
            template.Sthree.Title = cmd.CTitles[2];
            template.Sthree.Subtitle = cmd.CSubtitles[2];
            template.Sthree.Link = cmd.CLinks[2];
            if (cmd.CImage3 != null)
            {
                template.Sthree.Image = await _storage.SaveFile(cmd.CImage3, "showcases");
            }
            template.Sfour.Title = cmd.CTitles[3];
            template.Sfour.Subtitle = cmd.CSubtitles[3];
            template.Sfour.Link = cmd.CLinks[3];
            if (cmd.CImage4 != null)
            {
                template.Sfour.Image = await _storage.SaveFile(cmd.CImage4, "showcases");
            }

            _context.Update(template);

            await _context.SaveChangesAsync();

            
            return Ok();
        }

        public async Task Init()
        {
            var template = new ShowcaseTemplate()
            {
                Main = new ShowcaseBlock(),
                Mone = new ShowcaseBlock(),
                Mtwo = new ShowcaseBlock(),
                Sone = new ShowcaseBlock(),
                Stwo = new ShowcaseBlock(),
                Sthree = new ShowcaseBlock(),
                Sfour = new ShowcaseBlock()
            };
            
            _context.Add(template);
            await _context.SaveChangesAsync();
        }
    }
}
