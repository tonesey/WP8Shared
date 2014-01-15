using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Wp8Shared.Helpers;
using System.Collections.Generic;
using Microsoft.Phone.Reactive;
using System.Linq;
using System;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Wp8Shared.UserControls
{
    public partial class OtherApps
    {

        //Dictionary<string, Guid> _appsGuid = new Dictionary<string, Guid>();
        private Guid _curAppGuid = Guid.Empty;
        private List<MyHubTile> _knownApps = new List<MyHubTile>();
        private Genre _requiredGenre = Genre.Undefined;

        public OtherApps()
        {
            InitializeComponent();
            InitApps();
        }

        private void InitApps()
        {
            _knownApps = new List<MyHubTile>() {
                        new MyHubTile()
                        {
                            Tag = "Peppa Pig (ita)",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("12809954-9d34-4eea-ac65-45499a540210"),
                            Height = 175,
                            Width = 175,
                            Title = "Peppa Pig (ita)",
                            Source = new BitmapImage(new Uri("../Resources/PeppaPig_it.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Peppa Pig (eng)",
                            SupportedCultures = new List<string>() { "en" },
                            Guid = new Guid("4baa5f26-585a-4f02-afdd-3116e43125e5"),
                            Height = 175,
                            Width = 175,
                            Title = "Peppa Pig (eng)",
                            Source = new BitmapImage(new Uri("../Resources/PeppaPig_en.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Peppa Pig (es)",
                            SupportedCultures = new List<string>() { "es" },
                            Guid = new Guid("7a105e28-8fcf-4a35-85c9-0f3a311c9fa9"),
                            Height = 175,
                            Width = 175,
                            Title = "Peppa Pig (es)",
                            Source = new BitmapImage(new Uri("../Resources/PeppaPig_es.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Peppa Pig (pt)",
                            SupportedCultures = new List<string>() { "es" },
                            Guid = new Guid("137819c7-c4f7-4334-9966-8dc91f51c0e2"),
                            Height = 175,
                            Width = 175,
                            Title = "Peppa Pig (pt)",
                            Source = new BitmapImage(new Uri("../Resources/PeppaPig_pt.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Peppa Pig (ru)",
                            SupportedCultures = new List<string>() { "ru" },
                            Guid = new Guid("81ae7f1f-a14c-4a67-9126-19039793ccad"),
                            Height = 175,
                            Width = 175,
                            Title = "Peppa Pig (ru)",
                            Source = new BitmapImage(new Uri("../Resources/PeppaPig_ru.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "I Puffi",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("9f58b7d1-77c1-4f03-af44-514a69bf6722"),
                            Height = 175,
                            Width = 175,
                            Title = "I Puffi",
                            Source = new BitmapImage(new Uri("../Resources/Puffi.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "My Little Pony",
                            SupportedCultures = new List<string>() { "en" },
                            Guid = new Guid("85a37e84-9188-428b-b58b-9822cb16cb5c"),
                            Height = 175,
                            Width = 175,
                            Title = "My Little Pony",
                            Source = new BitmapImage(new Uri("../Resources/MyLittlePony.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Memessenger+",
                            SupportedCultures = new List<string>() { "it", "en" },
                            Guid = new Guid("dc0a8ce0-dd4f-4a92-b2b7-81c1709f389f"),
                            Height = 175,
                            Width = 175,
                            Title = "Memessenger+",
                            Source = new BitmapImage(new Uri("../Resources/Memessenger+.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
                        new MyHubTile()
                        {
                            Tag = "Memessenger",
                            SupportedCultures = new List<string>() { "it", "en" },
                            Guid = new Guid("c53e39aa-9248-466b-aaab-78931c23c158"),
                            Height = 175,
                            Width = 175,
                            Title = "Memessenger",
                            Source = new BitmapImage(new Uri("../Resources/Memessenger.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
                        new MyHubTile()
                        {
                            Tag = "Pingu",
                            SupportedCultures = new List<string>() { "it", "en", "pt", "es" },
                            Guid = new Guid("8ead178a-a84e-48d9-9e4b-4a8f4f7860e9"),
                            Height = 175,
                            Width = 175,
                            Title = "Pingu",
                            Source = new BitmapImage(new Uri("../Resources/Pingu.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "BanfiHits",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("eedf562f-c645-4a75-8222-92df4c0035fd"),
                            Height = 175,
                            Width = 175,
                            Title = "BanfiHits",
                            Source = new BitmapImage(new Uri("../Resources/BanfiHits.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
                        new MyHubTile()
                        {
                            Tag = "PozzettoHits",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("eeb4cba4-f648-4409-9e66-550651b879e1"),
                            Height = 175,
                            Width = 175,
                            Title = "PozzettoHits",
                            Source = new BitmapImage(new Uri("../Resources/PozzettoHits.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
                        new MyHubTile()
                        {
                            Tag = "MonnezzaHits",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("9a29f30b-0f15-482d-b383-a3b120daa28e"),
                            Height = 175,
                            Width = 175,
                            Title = "MonnezzaHits",
                            Source = new BitmapImage(new Uri("../Resources/MonnezzaHits.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
                        new MyHubTile()
                        {
                            Tag = "ScaleFinder",
                            SupportedCultures = new List<string>() { "it", "en", "es", "pt" },
                            Guid = new Guid("35827306-ffc5-41d6-ba7c-467e03b61c2f"),
                            Height = 175,
                            Width = 175,
                            Title = "ScaleFinder",
                            Source = new BitmapImage(new Uri("../Resources/ScaleFinder.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Music
                        },
                        new MyHubTile()
                        {
                            Tag = "Banane",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("d6ca939f-bfb9-4847-a3c2-284b36885a77"),
                            Height = 175,
                            Width = 175,
                            Title = "Banane in pigiama",
                            Source = new BitmapImage(new Uri("../Resources/Banane.png", UriKind.Relative)),
                            FontSize = 11,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.KidsAndFamily
                        },
                        new MyHubTile()
                        {
                            Tag = "Cinico TV",
                            SupportedCultures = new List<string>() { "it" },
                            Guid = new Guid("53d7c817-dff0-46da-be21-718b834fdf10"),
                            Height = 175,
                            Width = 175,
                            Title = "Cinico TV",
                            Source = new BitmapImage(new Uri("../Resources/CinicoTV.png", UriKind.Relative)),
                            FontSize = 12,
                            Style = (Style)this.Resources["HubTileStyle1"],
                            GroupTag = "apps",
                            Genre = Genre.Enterteninment
                        },
            };
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Animate();
        }

        public void Animate()
        {
            mainPanel.Children.Clear();

            var appsToShow = _knownApps.Where(a => a.Guid != _curAppGuid
                                                                    &&
                                                                    (a.SupportedCultures.Contains("en") ||
                                                                     a.SupportedCultures.Contains(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)));
            //per vederle tutte
            //var appsToShow = _knownApps.Where(a => a.Guid != _curAppGuid);


            if (_requiredGenre != Genre.Undefined)
            {
                appsToShow = appsToShow.Where(a => a.Genre == _requiredGenre);
            }

            appsToShow.ToObservable().OnTimeline(TimeSpan.FromSeconds(.2))
                                     .ObserveOnDispatcher()
                                     .Subscribe(AddItem);
        }

        public void AddItem(MyHubTile item)
        {
            try
            {
                mainPanel.Children.Add(item);
                item.Tap -= new EventHandler<System.Windows.Input.GestureEventArgs>(HubTile_Tap);
                item.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(HubTile_Tap);
            }
            catch (Exception)
            {
            }
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
                string guidToStr = (sender as MyHubTile).Guid.ToString();

                marketplaceDetailTask.ContentIdentifier = guidToStr;
                marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
                marketplaceDetailTask.Show();

            }
            catch (InvalidOperationException ignored)
            {

            }
        }

        public void SetRequiredGenre(Genre genre)
        {
            _requiredGenre = genre;
        }

        public void SetCurrentAppGuid(Guid appGuid)
        {
            _curAppGuid = appGuid;
        }

        public void FreezeAll()
        {
            HubTileService.FreezeGroup("apps");
        }

        public void UnFreezeAll()
        {
            HubTileService.UnfreezeGroup("apps");
            Animate();
        }


    }
}