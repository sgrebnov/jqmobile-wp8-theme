/*
 * Licensed under the Apache License, Version 2.0 (the "License")
 * http://www.apache.org/licenses/LICENSE-2.0 
 *
 * Copyright (c) 2011-2012, Sergey Grebnov
 */

using System;
using System.Runtime.Serialization;
using Microsoft.Phone.Tasks;
using CordovaClassLib = WP8CordovaClassLib;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// Represents command that allows the user to choose a date (day/month/year) or time (hour/minute/am/pm).
    /// </summary>
    public class DateTimePicker : CordovaClassLib.Cordova.Commands.BaseCommand
    {

        #region DateTimePicker Options

        /// <summary>
        /// Represents DateTimePicker options
        /// </summary>
        [DataContract]
        public class DateTimePickerOptions
        {
            /// <summary>
            /// Initial value for time or date
            /// </summary>
            [DataMember(IsRequired = false, Name = "value")]
            public DateTime Value { get; set; }   
         
                        /// <summary>
            /// Creates options object with default parameters
            /// </summary>
            public DateTimePickerOptions()
            {
                this.SetDefaultValues(new StreamingContext());
            }

            /// <summary>
            /// Initializes default values for class fields.
            /// Implemented in separate method because default constructor is not invoked during deserialization.
            /// </summary>
            /// <param name="context"></param>
            [OnDeserializing()]
            public void SetDefaultValues(StreamingContext context)
            {
                this.Value = DateTime.Now;
            }

        }
        #endregion

        /// <summary>
        /// Used to open datetime picker
        /// </summary>
        private DateTimePickerTask dateTimePickerTask;

        /// <summary>
        /// DateTimePicker options
        /// </summary>
        private DateTimePickerOptions dateTimePickerOptions;

        /// <summary>
        /// Triggers  special UI to select a date (day/month/year)
        /// </summary>
        public void selectDate(string options)
        {

            try
            {
                try
                {
                    this.dateTimePickerOptions = String.IsNullOrEmpty(options) ? new DateTimePickerOptions() : 
                        CordovaClassLib.Cordova.JSON.JsonHelper.Deserialize<DateTimePickerOptions>(options);

                }
                catch (Exception ex)
                {
                    this.DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.JSON_EXCEPTION, ex.Message));
                    return;
                }

                this.dateTimePickerTask = new DateTimePickerTask();
                dateTimePickerTask.Value = dateTimePickerOptions.Value;

                dateTimePickerTask.Completed += this.dateTimePickerTask_Completed;
                dateTimePickerTask.Show(DateTimePickerTask.DateTimePickerType.Date);
            }
            catch (Exception e)
            {
                DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.ERROR, e.Message));
            }
        }

        /// <summary>
        /// Triggers  special UI to select a time (hour/minute/am/pm).
        /// </summary>
        public void selectTime(string options)
        {

            try
            {
                try
                {
                    this.dateTimePickerOptions = String.IsNullOrEmpty(options) ? new DateTimePickerOptions() :
                       CordovaClassLib.Cordova.JSON.JsonHelper.Deserialize<DateTimePickerOptions>(options);

                }
                catch (Exception ex)
                {
                    this.DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.JSON_EXCEPTION, ex.Message));
                    return;
                }

                this.dateTimePickerTask = new DateTimePickerTask();
                dateTimePickerTask.Value = dateTimePickerOptions.Value;

                dateTimePickerTask.Completed += this.dateTimePickerTask_Completed;
                dateTimePickerTask.Show(DateTimePickerTask.DateTimePickerType.Time);
            }
            catch (Exception e)
            {
                DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.ERROR, e.Message));
            }
        }

        /// <summary>
        /// Handles datetime picker result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">stores information about current captured image</param>
        private void dateTimePickerTask_Completed(object sender, DateTimePickerTask.DateTimeResult e)
        {

            if (e.Error != null)
            {
                DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.ERROR));
                return;
            }            

            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    try
                    {
                        DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.OK, e.Value.Value.ToString()));
                    }
                    catch (Exception ex)
                    {
                        DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.ERROR, "Datetime picker error. " + ex.Message));
                    }
                    break;

                case TaskResult.Cancel:
                    DispatchCommandResult(new CordovaClassLib.Cordova.PluginResult(CordovaClassLib.Cordova.PluginResult.Status.ERROR, "Canceled."));
                    break;               
            }

            this.dateTimePickerTask = null;
        }       

    }
}