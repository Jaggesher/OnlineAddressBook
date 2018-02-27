// Write your JavaScript code.
var i = 0;

$(document).ready(function() {

    i = ($('#MultiplePhones > div').length - 1);

    $('#AddAnotherPhone').on('click', addAnotherPhone);

    $('#RemoveLastPhone').on('click', removeLastPhone);

    //$('#AllContactTable').DataTable();
    $('#contactTable').dataTable();

    // $('#contactTable').DataTable();

    $('[data-toggle="tooltip"]').tooltip();

});

function addAnotherPhone() {

    i++;
    $('#MultiplePhones').append('<div class="form-group"><label for="Phones_' + i + '__Phone">Phone</label><input class="form-control" type="text" data-val="true" data-val-maxlength="Phone Number cannot be longer than 20 characters." data-val-maxlength-max="20" id="Phones_' + i + '__Phone" name="Phones[' + i + '].Phone" value="" /><span class="text-danger field-validation-valid" data-valmsg-for="Phones[' + i + '].Phone" data-valmsg-replace="true"></span></div>');
    $('#RemoveLastPhone').fadeIn();

}

function removeLastPhone() {
    i--;
    $('#MultiplePhones').children().last().remove();
    if (i < 0) $('#RemoveLastPhone').fadeToggle("slow");

}