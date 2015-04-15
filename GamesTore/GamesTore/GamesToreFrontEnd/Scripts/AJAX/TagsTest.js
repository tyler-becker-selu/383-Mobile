/// <reference path="../jquery-1.8.2.intellisense.js" />

var relURL = sessionStorage['relURL'],
    authid = sessionStorage['xcmps383authenticationid'],
    key = sessionStorage['xcmps383authenticationkey'],
    myTag;

function getTags() {
    'use strict';
    $.ajax({
        url: relURL + 'api/Tags',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Tags:' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function getTag(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Tags/' + id,
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('get Tag: ' + data);
        myTag = data;
        $('#putTag').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function postTag() {
    'use strict';
    updateTag();
    $.ajax({
        url: relURL + 'api/Tags',
        type: 'POST',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myTag
    }).done(function (data, textStatus, jqXHR) {
        console.log('post Tag: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function putTag() {
    'use strict';
    updateTag();
    $.ajax({
        url: myTag.URL,
        type: 'PUT',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        },
        data: myTag
    }).done(function (data, textStatus, jqXHR) {
        console.log('put Tag: ' + data);
        $('#putTag').prop('disabled', function (i, v) { return !v; });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function deleteTag(id) {
    'use strict';
    $.ajax({
        url: relURL + 'api/Tags/' + id,
        type: 'DELETE',
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader('xcmps383authenticationid', authid);
            jqXHR.setRequestHeader('xcmps383authenticationkey', key);
        }
    }).done(function (data, textStatus, jqXHR) {
        console.log('delete Tag: ' + data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log(jqXHR.responseText || textStatus);
    });
}

function updateTag() {
    myTag.Name = $('#tagName').val();
}