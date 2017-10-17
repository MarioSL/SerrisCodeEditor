﻿using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class ModuleSheet : UserControl
    {
        bool isSelected = false, isInitialized = false; ModuleSheetNotification current_sheet = new ModuleSheetNotification();

        public ModuleSheet()
        {
            this.InitializeComponent();
        }

        private void SheetButton_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); }

        private void SheetButton_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if(DataContext != null)
            {
                current_sheet = (ModuleSheetNotification)DataContext;
                name_sheet.Text = current_sheet.sheetName;
                icon_sheet.Source = current_sheet.sheetIcon;

                if (current_sheet.sheetSystem)
                    close_sheet.Visibility = Visibility.Collapsed;

            }
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void GridButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if(!isSelected)
            {
                current_sheet = (ModuleSheetNotification)DataContext; current_sheet.type = ModuleSheetNotificationType.SelectSheet;
                Messenger.Default.Send(current_sheet);
            }
        }

        private void GridButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!current_sheet.sheetSystem)
                close_sheet.Visibility = Visibility.Visible;

            name_sheet.Visibility = Visibility.Visible;
            icon_sheet.Visibility = Visibility.Collapsed;
            GridButton.Width = 200;
        }

        private void GridButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!current_sheet.sheetSystem)
                close_sheet.Visibility = Visibility.Collapsed;

            name_sheet.Visibility = Visibility.Collapsed;
            icon_sheet.Visibility = Visibility.Visible;
            GridButton.Width = 32;
        }

        private void close_sheet_Click(object sender, RoutedEventArgs e)
        {
            current_sheet.type = ModuleSheetNotificationType.RemoveSheet;
            Messenger.Default.Send(current_sheet);
        }

        private void SetMessenger()
        {
            Messenger.Default.Register<ModuleSheetNotification>(this, (notification) =>
            {
                try
                {
                    switch (notification.type)
                    {

                        case ModuleSheetNotificationType.SelectSheet:
                            if(notification.id == current_sheet.id)
                            {
                                isSelected = true;
                                GridButton.BorderThickness = new Thickness(1);
                            }
                            else
                            {
                                isSelected = false;
                                GridButton.BorderThickness = new Thickness(0);
                            }
                            break;

                        case ModuleSheetNotificationType.UpdatedSheet:
                            if (notification.id == current_sheet.id)
                                current_sheet = notification;
                            break;
                    }
                }
                catch { }
            });

            if (!isInitialized)
            {
                current_sheet.type = ModuleSheetNotificationType.InitalizedSheet;
                Messenger.Default.Send(current_sheet);

                isInitialized = true;
            }

        }
    }
}