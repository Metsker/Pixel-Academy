using System;
using System.Collections;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace _Scripts.SharedOverall
{
    public class NotificationManager : MonoBehaviour
    {
        private const string ChannelId = "default_id";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
#if UNITY_ANDROID
            StartCoroutine(AndroidNotificator());
#elif UNITY_IOS
            StartCoroutine(IOSNotificator());
#endif
        }
        
#if UNITY_ANDROID
        private IEnumerator AndroidNotificator()
        {
            AndroidNotificationCenter.CancelAllNotifications();

            yield return LocalizationSettings.InitializationOperation;
            yield return new WaitUntil(LanguageManager.IsLanguageInitialized);

            var defaultNotificationChannel = new AndroidNotificationChannel {Id = ChannelId, Name = "Notifications Channel", Description = "Reminder notifications", Importance = Importance.High};

            AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

            var defaultNotification = new AndroidNotification
            {
                Title = Dictionaries.GetLocalizedString("Title"),
                Text = Dictionaries.GetLocalizedString("Body"),
                SmallIcon = "default",
                FireTime = DateTime.Today.AddHours(18),
                RepeatInterval = TimeSpan.FromDays(2)
            };

            AndroidNotificationCenter.SendNotification(defaultNotification, ChannelId);

            AndroidNotificationCenter.OnNotificationReceived += _ =>
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(defaultNotification, ChannelId);
            };
        }
#elif UNITY_IOS
            private IEnumerator IOSNotificator()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();

            yield return LocalizationSettings.InitializationOperation;
            yield return new WaitUntil(LanguageManager.IsLanguageInitialized);

            var intervalTrigger = new iOSNotificationTimeIntervalTrigger
            {
                TimeInterval = TimeSpan.FromDays(2),
                Repeats = true
            };

            var notification = new iOSNotification
            {
                Identifier = "_notification_01",
                Title = Dictionaries.GetLocalizedString("Title"),
                Body = Dictionaries.GetLocalizedString("Body"),
                Subtitle = Dictionaries.GetLocalizedString("SubtitleIOS"),
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = intervalTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);

            iOSNotificationCenter.OnNotificationReceived += _ =>
            {
                iOSNotificationCenter.RemoveDeliveredNotification(notification.Identifier);
                iOSNotificationCenter.ScheduleNotification(notification);
            };
        }
#endif
    }
}

