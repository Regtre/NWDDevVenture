﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NWDAppRuntime.Middle;
using NWDAppRuntime.Middle.Facades;
using NWDFoundation.Models;
using NWDMauiRuntime.Middle.Managers;
using NWDWebRuntime.Models;

namespace NWDMaui.ViewModels;

public class ProfileViewModel: INotifyPropertyChanged
{
    private readonly INWDAccountManager _AccountManager; 
    public ProfileViewModel(INWDAccountManager sAccountManager)
    {
        _AccountManager = sAccountManager;
        LoadData();
    }
    public  ObservableCollection<NWDAccountSign> Signs{ get; private set; }
    public ObservableCollection<NWDAccountService> Services{ get; private set; }
    public ObservableCollection<NWDAccountInfo> AccountInfos { get; private set; }
    public ObservableCollection<NWDPlayerData> PlayerData { get; private set; }
    public NWDAccount Account { get; private set; }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void LoadData()
    {
        Signs = new ObservableCollection<NWDAccountSign>(_AccountManager.GetSigns());
        Services = new ObservableCollection<NWDAccountService>(_AccountManager.GetServices());
        /*
        AccountInfos = new ObservableCollection<NWDAccountInfo>(_AccountManager.GetAccountInfos());
        */
        Account= NWDAccountManager.GetAccount();
        NWDDataManager.FastSync();
        PlayerData = new ObservableCollection<NWDPlayerData>(NWDDataManager.GetAllData<NWDPlayerData>());
    }
}