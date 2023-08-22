using System.Net;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Tools;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;

namespace NWDWebStandard.Controllers;

public class NWDNotificationController : NWDBasicController<NWDNotificationController>
{
    public IActionResult GetNotifications()
    {
        List<NWDNotification> tNotifications =
            NWDNotificationManager.GetNotificationForAccountSortByDateDesc(NWDAccountWebManager
                .GetAccountInContext(HttpContext).Reference);
        return PartialView("_NavBar_Account_Notification", new NWDNotificationFilteredLists(tNotifications));
    }

    public IActionResult GenerateNotif()
    {
        NWDNotification? tNotification = new NWDNotification
        {
            Body = "Test" + DateTime.Now.ToShortTimeString(),
            Icon = "fa fa-bell",
            CreationDate = DateTime.Now,
            AccountReference = NWDAccountWebManager.GetAccountInContext(HttpContext).Reference
        };
        NWDNotificationManager.Save(tNotification);
        return RedirectToAction("Index", "NWDAccount");
    }

    public IActionResult GenerateNotifWithLink()
    {
        NWDNotification? tNotification = new NWDNotification()
        {
            Body = "Test" + DateTime.Now.ToShortTimeString(),
            Icon = "fa fa-bell",
            CreationDate = DateTime.Now,
            Link = "https://www.google.com",
            AccountReference = NWDAccountWebManager.GetAccountInContext(HttpContext).Reference
        };
        NWDNotificationManager.Save(tNotification);
        return RedirectToAction("Index", "NWDAccount");
    }

    public IActionResult GenerateNotifWithModal()
    {
        string randomString = "exampleModal"+NWDRandom.RandomString(9);
        NWDNotification? tNotification = new NWDNotification()
        {
            Body = "Test" + DateTime.Now.ToShortTimeString(),
            Icon = "fa fa-bell",
            CreationDate = DateTime.Now,
            ModalId = randomString,
            AccountReference = NWDAccountWebManager.GetAccountInContext(HttpContext).Reference,
            Modal = new NWDModal()
            {
                Id = randomString,
                Title = "Test",
                Body = "Test",
                ButtonFooterPositiveLabel = "OK",
                ButtonFooterNegativeLabel = "Cancel"
            }
        };
        NWDNotificationManager.Save(tNotification);
        return RedirectToAction("Index", "NWDAccount");
    }

    public IActionResult MarkAsRead(string Reference)
    {
        NWDNotification? tNotification = null;
        if (ulong.TryParse(Reference, out ulong tReference))
        {
            tNotification =
                NWDNotificationManager.GetByReference(tReference);
            if (tNotification != null)
            {
                tNotification.Read = true;
                NWDNotificationManager.Save(tNotification);
            }
        }


        if (tNotification != null && !string.IsNullOrEmpty(tNotification.Link))
        {
            return Redirect(tNotification.Link);
        }
        else
        {
            return RedirectToAction("GetNotifications");
        }
    }

    public IActionResult MarkAllAsRead()
    {
        List<NWDNotification> tNotifications =
            NWDNotificationManager.GetNotificationForAccountSortByDateDesc(NWDAccountWebManager
                .GetAccountInContext(HttpContext).Reference);
        foreach (NWDNotification tNotification in tNotifications)
        {
            tNotification.Read = true;
            NWDNotificationManager.Save(tNotification);
        }

        return RedirectToAction("GetNotifications");
    }

    public IActionResult GetNewModalsFromNotif()
    {
        return PartialView("_Modals",
            new NWDNotificationFilteredLists(
                NWDNotificationManager
                .GetNotificationForAccountSortByDateDesc(NWDAccountWebManager.GetAccountInContext(HttpContext).Reference))
                .GetModalNotification()
                .Where(sItem => sItem.Read == false)
                .Select(tItem => tItem.Modal));
    }
}