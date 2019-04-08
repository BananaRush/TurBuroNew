function DeleteAlretMessage() {
	return confirm("Вы действительно хотите удалить элемент?");
}

function DeleteImg() {
    document.querySelector('.image_file').value = '';
    $('input[name="ImageName"]').val("");
}

function NewDeleteImg() {
    $(event.target).closest("li").find(".image_file").attr("value", "");
    $(event.target).closest("li").find(".img-thumbnail").attr("src", "");
}

function AddImg() {
    $(event.target).closest("li").find(".img-thumbnail").attr("src", URL.createObjectURL(event.target.files[0]));
}

function AddImageNumber(id) {
    //$('#IdAlbom').val(id);
    document.querySelector('#IdAlbom').value = id;
}

function DeleteImages(id, btn) {
    btn.href += "?id=" + id;
    var elm = "#Albom_" + id;
    $(elm).find($('input:checkbox:checked').each(function () {
        btn.href += "&idimg=" + $(this).val();
    }));
}

function AddSection(id) {
    document.querySelector('#IdSection').value = id;
}

function EditSection(id) {
    var name = "#label_" + id;
    document.querySelector('#IdSectionEdit').value = id;
    document.querySelector('#SectionPlan').value = $(name).text();
}

$('.dropdown-menu').on('click', function (event) {
    // The event won't be propagated up to the document NODE and 
    // therefore delegated events won't be fired
    event.stopPropagation();
});
