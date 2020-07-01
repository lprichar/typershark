document.onkeypress = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('TypeShark2.Client', 'JsKeyPress', evt.key);
}