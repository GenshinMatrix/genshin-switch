using AutoMapper;
using GenshinSwitch.ViewModels.Contacts;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.DailyNote;
using Xunkong.Hoyolab.SpiralAbyss;

namespace GenshinSwitch.Models.Contacts;

public class ContactMapperProvider
{
    public static IMapper Service { get; }

    static ContactMapperProvider()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<SignInInfo, SignInInfoViewModel>();
            cfg.CreateMap<DailyNoteInfo, DailyNoteInfoViewModel>();
            cfg.CreateMap<Transformer, TransformerViewModel>();
            cfg.CreateMap<TransformerRecoveryTime, TransformerRecoveryTimeViewModel>();
            cfg.CreateMap<SpiralAbyssInfo, SpiralAbyssInfoViewModel>();
            cfg.CreateMap<SpiralAbyssRank, SpiralAbyssRankViewModel>();
            cfg.CreateMap<SpiralAbyssFloor, SpiralAbyssFloorViewModel>();
            cfg.CreateMap<SpiralAbyssLevel, SpiralAbyssLevelViewModel>();
            cfg.CreateMap<SpiralAbyssBattle, SpiralAbyssBattleViewModel>();
        });
        Service = config.CreateMapper();
    }

    public static TDestination MapDefault<TSource, TDestination>(TSource source, TDestination destination)
    {
        return Map(source, destination, cfg =>
        {
            cfg.CreateMap<TSource, TDestination>();
        });
    }

    public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMapperConfigurationExpression> configure = null!)
    {
        if (configure != null)
        {
            MapperConfiguration config = new(configure);
            IMapper mapper = config.CreateMapper();
            return mapper.Map(source, destination);
        }
        return Service.Map(source, destination);
    }
}
