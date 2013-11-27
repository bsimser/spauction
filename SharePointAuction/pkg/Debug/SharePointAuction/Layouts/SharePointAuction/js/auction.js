function auctionOpenModalURL(url, title, width, height, callbackType, isMax) {
    var options = SP.UI.$create_DialogOptions();
    options.url = url;
    options.title = title;
    options.width = width;
    options.height = height;
    options.showMaximized = isMax;

    if (callbackType == 'NotificationCallback') {
        options.dialogReturnValueCallback = NotificationCallback;
    }
    else if (callbackType == 'SilentCallback') {
        options.dialogReturnValueCallback = SilentCallback;
    }
    else if (callbackType == 'refreshCallback') {
        options.dialogReturnValueCallback = refreshCallback;
    }

    SP.UI.ModalDialog.showModalDialog(options);
}

function NotificationCallback(dialogResult, returnValue) {
    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation succeeded', false);
    }
    else if (dialogResult == SP.UI.DialogResult.cancel) {
        SP.UI.Notify.addNotification('Operation cancelled', false);
    }
    else if (dialogResult == SP.UI.DialogResult.invalid) {
        SP.UI.Notify.addNotification('Operation invalid', false);
    }

    alert(dialogResult);
}

function SilentCallback(dialogResult, returnValue) {
    alert(dialogResult);
}

function refreshCallback(dialogResult, returnValue) {
    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
    }
}

function newAuctionItem() {
    var options = {
        url: L_Menu_BaseUrl + "/Lists/SPAuctionItems/NewForm.aspx",
        title: "New Auction Item",
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: refreshCallback
    };
    SP.UI.ModalDialog.showModalDialog(options);
}

function editAuctionItem(id) {
    var editOptions = {
        url: L_Menu_BaseUrl + "/Lists/SPAuctionItems/EditForm.aspx",
        title: "Edit Auction Item",
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: refreshCallback
    };
    editOptions.url = L_Menu_BaseUrl + "/Lists/SPAuctionItems/EditForm.aspx?ID=" + id;
    SP.UI.ModalDialog.showModalDialog(editOptions);
}
