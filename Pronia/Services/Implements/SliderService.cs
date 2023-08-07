using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Models;
using Pronia.ViewModels.SliderVMs;
using P137Pronia.Services.Interfaces;

namespace Pronia.Services.Implements;

public class SliderService : ISliderService
{
    private readonly ProniaDBContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IFileService _fileService;

    public SliderService(ProniaDBContext context,
        IWebHostEnvironment env,
        IFileService fileService)
    {
        _context = context;
        _env = env;
        _fileService = fileService;
    }
    public async Task Create(CreateSliderVM sliderVM)
    {
        await _context.Sliders.AddAsync(new Slider
        {
            ImageUrl = await _fileService.UploadAsync(sliderVM.ImageFile,Path.Combine("assets","imgs"),"image",2),
            Title = sliderVM.Title,
            ButtonText = sliderVM.ButtonText,
            Offer = sliderVM.Offer,
            Desc = sliderVM.Desc
        });
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int? id)
    {
        var entity=await GetById(id);
        _context.Sliders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Slider>> GetAll()
    {
        return await _context.Sliders.ToListAsync();
    }

    public async Task<Slider> GetById(int? id)
    {
        if (id < 1 || id == null) throw new ArgumentException();
        var entity = await _context.Sliders.FindAsync(id);
        if (entity == null) throw new ArgumentNullException();
        return entity;
    }

    public async Task Update(UpdateSliderVM sliderVM)
    {
        var entity = await GetById(sliderVM.Id);
        entity.Title = sliderVM.Title;
        entity.Offer = sliderVM.Offer;
        entity.Desc = sliderVM.Desc;
        entity.ButtonText = sliderVM.ButtonText;
        //entity.ImageUrl = sliderVM.ImageUrl;
        await _context.SaveChangesAsync();
    }

    Task<ICollection<Slider>> ISliderService.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<ICollection<Slider>> ISliderService.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<Slider> ISliderService.GetById(int? id)
    {
        throw new NotImplementedException();
    }

    Task<Slider> ISliderService.GetById(int? id)
    {
        throw new NotImplementedException();
    }
}

