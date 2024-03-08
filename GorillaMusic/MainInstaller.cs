using GorillaMusic.Behaviours;
using GorillaMusic.Behaviours.Loaders;
using UnityEngine;
using Zenject;

namespace GorillaMusic
{
    public class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HoldableManager>().FromNewComponentOn(GetPlayer()).AsSingle();
            Container.BindInterfacesAndSelfTo<MusicPlayer>().FromNewComponentOn(GetPlayer()).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioLoader>().FromNewComponentOn(GetPlayer()).AsSingle();
            Container.Bind<AssetLoader>().AsSingle();
        }

        public GameObject GetPlayer() => GorillaLocomotion.Player.Instance.gameObject;
    }
}