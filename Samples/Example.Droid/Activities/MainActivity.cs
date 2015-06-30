﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Support.Fragging.Presenter;
using Cirrious.MvvmCross.ViewModels;
using Example.Core.ViewModels;
using Example.Droid.Fragments;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Example.Droid.Activities
{
    [Activity(
        Label = "Examples",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        Name = "example.droid.activities.MainActivity"
    )]
    public class MainActivity : MvxCachingFragmentActivityCompat<MainViewModel>, IMvxFragmentHost
    {
        public DrawerLayout drawerLayout;

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        public MainActivity ()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate (bundle);

            RegisterForDetailsRequests(bundle);

            SetContentView (Resource.Layout.activity_main);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            ViewModel.ShowMenu ();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
            case Android.Resource.Id.Home:
                drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void RegisterForDetailsRequests(Bundle bundle)
        {
            RegisterFragment<MenuFragment, MenuViewModel>(typeof(MenuViewModel).Name, bundle, new MenuViewModel());
            RegisterFragment<ExamplesFragment, ExamplesViewModel>(typeof(ExamplesViewModel).Name, bundle, new ExamplesViewModel());
        }

        public void RegisterFragment<TFragment, TViewModel>(string tag, Bundle args, IMvxViewModel viewModel = null) 
            where TFragment : IMvxFragmentView where TViewModel : IMvxViewModel
        {
            var customPresenter = Mvx.Resolve<IMvxFragmentsPresenter>();
            customPresenter.RegisterViewModelAtHost<TViewModel>(this);
            RegisterFragment<TFragment, TViewModel>(tag);
        }

        public bool Show (MvxViewModelRequest request, Bundle bundle)
        {
            if (request.ViewModelType == typeof(MenuViewModel)) {
                ShowFragment (request.ViewModelType.Name, Resource.Id.navigation_frame, bundle);
                return true;
            }
            else
            {
                ShowFragment (request.ViewModelType.Name, Resource.Id.content_frame, bundle);
                return true;
            }
        }
    }
}

