

function ShowMyAlertB(sElement)
{
    alert(' my element is ...' + sElement);
    console.log($(sElement));
}
function clearSelection() {
    if(document.selection && document.selection.empty) {
        document.selection.empty();
    } else if(window.getSelection) {
        var sel = window.getSelection();
        sel.removeAllRanges();
    }
}
function AddShowAndCopySourceCodeToElement(sElement, sInner, sBefore)
{
    if (sElement.querySelector("#copy-source") == null) {
        clearSelection();
        let tDiv = document.createElement("div");
        tDiv.className ='text-end'

        let tDivShow = document.createElement("div");
        tDivShow.className ='card';
        tDiv.appendChild(Object.assign(tDivShow));
        let tDivHeader = document.createElement("div");
        tDivHeader.className ='card-header border-primary';
        tDivHeader.innerText = 'Source code';
        tDivShow.appendChild(Object.assign(tDivHeader));
        let tDivBody= document.createElement("div");
        tDivBody.className ='card-body bg-dark';
        tDivShow.appendChild(Object.assign(tDivBody));
        let tDivCode = document.createElement("code");
        tDivCode.className ='text-start';
        tDivBody.appendChild(Object.assign(tDivCode));
        let tDivPre = document.createElement("pre");
        tDivCode.appendChild(Object.assign(tDivPre));
        let tShowButton = document.createElement("button");
        tShowButton.className = 'btn btn-s btn-primary mb-1 mt-1 bi bi-eye ms-1';
        tShowButton.innerHTML = ' show source code';
        tDiv.appendChild(Object.assign(tShowButton));
        tDivShow.hidden =true;
        tShowButton.onclick = function() {
            if (tDivShow.hidden == true)
            {
                tShowButton.innerHTML = ' hide source code';
                tShowButton.className = 'btn btn-s btn-primary mb-1 mt-1 bi bi-eye-slash ms-1';
                tDivShow.hidden =false;
                sElement.hidden=true;
            }
            else
            {
                tShowButton.innerHTML = ' show source code';
                tShowButton.className = 'btn btn-s btn-primary mb-1 mt-1 bi bi-eye ms-1';
                tDivShow.hidden =true;
                sElement.hidden=false;
            }
        };
        let tCopyButton = document.createElement("button");
        tCopyButton.className = 'btn btn-s btn-primary mb-1 mt-1 bi bi-clipboard ms-1';
        tCopyButton.innerHTML = ' copy source code';
        var tContent = sElement.outerHTML;
        if (sInner == true)
        {
            tContent = sElement.innerHTML;
        }
        // tContent = tContent.replace(" ondblclick=\"AddShowAndCopySourceCodeToElement(this, false, false);\"", "");
        // tContent = tContent.replace(" ondblclick=\"AddShowAndCopySourceCodeToElement(this, false, true);\"", "");
        // tContent = tContent.replace(" ondblclick=\"AddShowAndCopySourceCodeToElement(this, true, false);\"", "");
        // tContent = tContent.replace(" ondblclick=\"AddShowAndCopySourceCodeToElement(this, true, true);\"", "");
        
        tContent = tContent.replace(/ ondblclick=\"(.*)\"/g, "");
        tContent = tContent.replace(/<!-- (<span class="fas fa-shield-alt"><\/span>) Font Awesome fontawesome.com -->/g, "$1");
        tContent = tContent.replace(/<svg (.*)>(.*)<\/svg>/g, "");
        tDivPre.innerText = tContent;
        tCopyButton.onclick = function() {
            navigator.clipboard.writeText(tContent);
            tCopyButton.className = 'btn btn-s btn-outline-primary mb-1 mt-1 bi bi-clipboard-check ms-1';
            tCopyButton.innerHTML = ' source code copied';
        };
        tDiv.appendChild(Object.assign(tCopyButton));
        let parentDiv = sElement.parentNode;
        if (sBefore == true) {
            parentDiv.insertBefore(tDiv, sElement);
        }
        else
        {
            sElement.parentNode.insertBefore(tDiv, sElement.nextSibling);
        }
        let tDivNoDupplicate = document.createElement("span");
        tDivNoDupplicate.id  ='copy-source';
        sElement.appendChild(tDivNoDupplicate);
    }
    else
    {
        //alert('element is present');
    }
}


function AddCopySourceCodeToElement(sElement, sInner, sBefore)
{
    if (sElement.querySelector("#copy-source") == null) {
        let tDiv = document.createElement("div");
        tDiv.className ='text-end'
        let tCopyButton = document.createElement("button");
        tCopyButton.className = 'btn btn-s btn-primary mb-1 mt-1 bi bi-clipboard';
        tCopyButton.innerHTML = ' copy source code';
        var tContent = sElement.outerHTML;
        if (sInner == true)
        {
            tContent = sElement.innerHTML;
        }
        tCopyButton.onclick = function() {
            navigator.clipboard.writeText(tContent);
            tCopyButton.className = 'btn btn-s btn-outline-primary mb-1 mt-1 bi bi-clipboard-check';
            tCopyButton.innerHTML = ' source code copied';
            //alert(tElement.outerHTML);
        };
        tDiv.appendChild(Object.assign(tCopyButton));
        let parentDiv = sElement.parentNode;
        if (sBefore == true) {
            parentDiv.insertBefore(tDiv, sElement);
        }
        else
        {
            sElement.parentNode.insertBefore(tDiv, sElement.nextSibling);
        }
        let tDivNoDupplicate = document.createElement("span");
        tDivNoDupplicate.id  ='copy-source';
        sElement.appendChild(tDivNoDupplicate);
    }
    else
    {
        //alert('element is present');
    }
}

// var ResultPostOperationList = [];
// var ResultPostOperationContentToReplace = '';
// function ResultPostOperation(sContentToReplaceID) {
//     ResultPostOperationContentToReplace = sContentToReplaceID;
//     ResultPostOperationList.forEach(sElement => eval(sElement));
// }
function ShowOrHiddeContent(sContent, sShow) {
    var tRoot = $('#'+sContent);
    if (sShow === true)
    {
        tRoot.show();
    }
    else
    {
        tRoot.hide();
    }
}

function CopyDivContentInClipboard(sDiv, sButton) {
    var copyText = document.getElementById(sDiv);
    var Button = document.getElementById(sButton);
    copyText.select();
    navigator.clipboard.writeText(copyText.value);
    alert(copyText.value);
}

function startRequestAndReplaceContent(sContentToReplaceID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    var tSpinner = tRoot.children('#spinner');
    tSpinner.show();
    tContent.hide();
    setTimeout(function(){
        $.get($('#'+sContentToReplaceID).data('url'), function (data) {
            //alert(data);
            tSpinner.hide();
            tContent.html(data);
            tContent.show();
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}
function refreshRequestAndReplaceContent(sContentToReplaceID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    var tSpinner = tRoot.children('#spinner');
    tSpinner.show();
    tContent.hide();
    setTimeout(function(){
        $.get($('#'+sContentToReplaceID).data('refresh'), function (data) {
            //alert(data);
            tSpinner.hide();
            tContent.html(data);
            tContent.show();
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}
function getRequestAndReplaceContent(sContentToReplaceID, sUrl) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    var tSpinner = tRoot.children('#spinner');
    tSpinner.show();
    tContent.hide();
    setTimeout(function(){
        $.get(sUrl, function (data) {
            //alert(data);
            tSpinner.hide();
            tContent.html(data);
            tContent.show();
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}
function postRequestAndReplaceContent(sContentToReplaceID, sUrl, sFormID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    var tSpinner = tRoot.children('#spinner');
    tSpinner.show();
    tContent.hide();
    setTimeout(function(){
        $.post(sUrl, $('#'+sFormID).serialize(), function (data) {
            //alert(data);
            tSpinner.hide();
            tContent.html(data);
            tContent.show();
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}

function postRequestAndRefresh(sContentToReplaceID, sUrl, sFormID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    var tSpinner = tRoot.children('#spinner');
    tSpinner.show();
    tContent.hide();
    setTimeout(function(){
        $.post(sUrl, $('#'+sFormID).serialize(), function (data) {
            //alert(data);
            tSpinner.hide();
            tContent.html(data);
            tContent.show();
            location.reload();
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}

function getRequestAndReplaceContentWithoutHideShow(sContentToReplaceID, sUrl, sFormID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    setTimeout(function(){
        $.post(sUrl, $('#'+sFormID).serialize(), function (data) {
            //alert(data);
            tContent.html(data);
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}
function getRequestAndAppendContentWithoutHideShow(sContentToReplaceID, sUrl, sFormID) {
    var tRoot = $('#'+sContentToReplaceID);
    var tContent = tRoot.children('#content_to_replace');
    setTimeout(function(){
        $.post(sUrl, $('#'+sFormID).serialize(), function (data) {
            //alert(data);
            tContent.append(data);
        });}, 100);
    //ResultPostOperation(sContentToReplaceID);
}