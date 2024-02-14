using CustomMusic.Behaviours;
using CustomSpeakerMusic.Behaviours;
using UnityEngine;
using Zenject;

namespace CustomMusic
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MusicPlayer>().FromNewComponentOn(GetPlayer()).AsSingle();
            Container.BindInterfacesAndSelfTo<AudioLoader>().FromNewComponentOn(GetPlayer()).AsSingle();
            Container.Bind<AssetLoader>().AsSingle();
        }

        public GameObject GetPlayer() => GorillaLocomotion.Player.Instance.gameObject;
    }
}