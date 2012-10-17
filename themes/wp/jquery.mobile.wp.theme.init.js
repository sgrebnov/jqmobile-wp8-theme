/*
 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

// special fix to have body content fit the entire browser area
function bodyMinHeightFix() {
    // portrait mode only
    if(window.innerHeight <= window.innerWidth) return;

    var zoomFactorW = document.body.clientWidth / screen.availWidth;

    // default value (web browser app)
    var addrBarH = 72;

    // no app bar in web view control
    if (typeof window.external.Notify !== "undefined") {
        addrBarH = 0;
    }

    var divHeightInDoc = (screen.availHeight-addrBarH) * zoomFactorW;
    //$("body")[0].style.minHeight = divHeightInDoc + 'px';

    var page  = $("div[data-role='page']");
    if (page.length > 0)
        page[0].style.setProperty("min-height", divHeightInDoc + "px", 'important');

}

$(document).ready(function(){
    // disable transitions effects by default	
    $.mobile.defaultDialogTransition = 'none';
    $.mobile.defaultPageTransition = 'none';

    if (!$.browser.msie){
    	return;
    }
    
    // ie specific logic and fixes

    $.mobile.pushStateEnabled = false;
    
    var version = parseInt($.browser.version);
    $(document.body).addClass('ui-ie' + version);

    if(version < 10){
        bodyMinHeightFix();    
    } else {
        $.mobile.defaultDialogTransition = 'slide';
        $.mobile.defaultPageTransition = 'slide';
    }     
});