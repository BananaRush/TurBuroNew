
// Элемент добавляем?
var IsAddElement = false;
var UIElementSelect = null;
var UIElementSelectPoint = null;
var listFiles = 
// Сброс курсора
function ResetCursor() {
    IsAddElement = false;
    document.body.style.cursor = "inherit";
    UIElementSelect = null;
}

// Двигаем элементы стрелочкой
document.onkeydown = function KeyHock(e) {
    if (UIElementSelectPoint !== null)
    {
        let x = $(UIElementSelectPoint).attr('data-x');
        let y = $(UIElementSelectPoint).attr('data-y');
  
        // Вверх
        if (e.keyCode === 38) {
            if (y - 1 >= 0) {
                y -= 1;
            }
        }

        // Вниз
        if (e.keyCode === 40) {
            if (y + 1 <= 1920) {
                ++y;
            }
        }

        //Влево
        if (e.keyCode === 37) {
            if (x - 1 >= 0) {
                x -= 1;
            }
        }

        // Вправо
        if (e.keyCode === 39) {
            if (x + 1 <= 1080) {
                ++x;
            }
        }

        // translate the element
        $(UIElementSelectPoint).css.webkitTransform =
            $(UIElementSelectPoint).css("transform", 'translate(' + x + 'px, ' + y + 'px)');

        // update the posiion attributes
        $(UIElementSelectPoint).attr('data-x', x);
        $(UIElementSelectPoint).attr('data-y', y);

        if (e.keyCode === 46) {
            // Берем индификатор
            let infify = $(UIElementSelectPoint).attr("Indify");
            $(UIElementSelectPoint).remove();
            let elm = $(".button-ui").find('[Indify="' + infify + '"]').last().show();
        }
    }

    if (e.keyCode === 46 || e.keyCode === 39 || e.keyCode === 37 || e.keyCode === 40 || e.keyCode === 38) {
        return false;
        e.cancelBubble = true;
        e.returnValue = false;
    }
}

//Флажок который гласит о видимости элемента при навигации
$("#page-edit-visibly").change(function () {
    if (UIElementSelectPoint != null) {
        if (this.checked) {
            $(UIElementSelectPoint).attr("IsNavVisibility", "true");
        }
        else {
            $(UIElementSelectPoint).attr("IsNavVisibility", "false");
        }
    }
});

// Происходит при клике по элементу панели
$('.UIElementAdd').click(function () {
    document.body.style.cursor = "pointer";
    UIElementSelect = $(this);
    IsAddElement = true;
});

// Обновляем свойства элемента
function ResizeDragClick (e) {
    UIElementSelectPoint = e;
    $("#menu-page-edit").find('input').each(function (i, v) {
        
        let attr = $(v).attr("css");
        let val;
        if (attr) {
            // Если двигаем
            if (attr == "data-x" || attr == "data-y") {
                if (attr == "data-x") {
                    $(v).val($(UIElementSelectPoint).attr("data-x"));
                }
                if (attr == "data-y") {
                    $(v).val($(UIElementSelectPoint).attr("data-y"));
                }
            }
            else {
                // Если атрибут равнен свойству
                let IsFlag = true;

                if (attr == "color" || attr == "background-color") {
                    $(v).val(rgb2hex($(UIElementSelectPoint).css(attr)));
                    IsFlag = false;
                }

                if (attr == "font-family") {
                    console.log($(UIElementSelectPoint).css(attr));
                    $('#visual-mng-font').val("arial").trigger('chosen:updated');
                }

                if (IsFlag) {
                    $(v).val($(UIElementSelectPoint).css(attr));
                }
            }
        }
    });

    // Скрывем показываем элементы
    if ($(UIElementSelectPoint).attr("ElementType") == "ButtonNav") {
        $("#registration-div").show();
    }
    else {
        $("#registration-div").hide();
    }

    if ($(UIElementSelectPoint).attr("ElementType") == "NavigationFrame") {
        $("#element-chack-nav").hide();
    }
    else {
        $("#element-chack-nav").show();
    }
    
    if ($(UIElementSelectPoint).attr("IsNavVisibility") == "false") {
        $("#page-edit-visibly").prop('checked', false);
    }
    else {
        $("#page-edit-visibly").prop('checked', true);
    }

    if ($(UIElementSelectPoint).attr("elementtype") == "Logotype"
        || $(UIElementSelectPoint).attr("elementtype") == "ButtonNav"
        || $(UIElementSelectPoint).attr("elementtype") == "BackButton") {
        $("#settings-div").show();
    }
    else {
        $("#settings-div").hide();
    }
}  

// Флажок выбора фона
$("#visual-mng-font").change(function () {
    let elm = $(this).parent().find("input[css=font-family]").last();
    $(elm).val($(this).val());
    $(elm).trigger('input');
});

// Происходит при клике на поле
$('#wapper').click(function (e) {
    document.body.style.cursor = "inherit";
    let PossY = e.offsetY;
    let PossX = e.offsetX;

    // Если флаг добавления элемента и вы деленный элемент не ноль
    if (IsAddElement && UIElementSelect !== null) {
        let elmType = $(UIElementSelect).find(".elm").last().val();
        let indify = $(UIElementSelect).attr("Indify");
        let elementType = $(UIElementSelect).attr("ElementType");
        let buttonNavId = $(UIElementSelect).attr("ButtonNavId");
        let fullHtml = '';
        let htmlElm = '';

        htmlElm = '<div IsNavVisibility="false" ButtonNavId="'
            + buttonNavId +
            '" Indify="'
            + indify +
            '"ElementType='
            + elementType +
            ' onclick="ResizeDragClick(this);" class="resize-drag '
            + elementType +
            '" style="transform: translate('
            + PossX + 'px, ' + PossY + 'px);" data-x='
            + PossX + ' data-y='
            + PossY + '>'
            /*+ fullHtml +*/ '</div>';

        $("#wapper").append(htmlElm);
        let elm = $("#wapper").children().last();
        $(UIElementSelect).children().clone().appendTo(elm);

        if (elementType != "Logotype")
            $(UIElementSelect).hide();
        ResizeDragClick(elm);
    }

    IsAddElement = false;
});

// Происходит при изменении значений в редакторе
$(document).ready(function () {
    $('.input-page-edit').on('input', function () {
        // Получаем текущее значение
        var msg = $(this).val();
        var attr = $(this).attr("css");

        if (attr == "data-x" || attr == "data-y") {
            if (attr == "data-x") {
                $(UIElementSelectPoint).attr("data-x", msg);
            }

            if (attr == "data-y") {
                $(UIElementSelectPoint).attr("data-y", msg);
            }
        }
        else {
            $(UIElementSelectPoint).css(attr, msg);
        }

        let x, y;
        x = $(UIElementSelectPoint).attr("data-x");
        y = $(UIElementSelectPoint).attr("data-y");

        UIElementSelectPoint.style.webkitTransform =
            UIElementSelectPoint.style.transform =
            'translate(' + x + 'px, ' + y + 'px)';
    });
});

// отправка
function ConstructElmSave(){
    var arr = [];
    $("#wapper").children().each(function (e, v) {

        let elm = {
            Id: v.id,
            IsNavVisibility: v.getAttribute("IsNavVisibility"),
            ButtonNavId: v.getAttribute("ButtonNavId"),
            ElementType: v.getAttribute("ElementType"),
            Top: v.getAttribute('data-y').replace('px', ''),
            Left: v.getAttribute('data-x').replace('px', ''),
            Width: v.clientWidth,
            Height: v.clientHeight,
            Type: v.getAttribute("type-elm"),
            FileImg: $(v).find(".img_source").last().attr("src"),
            ImageName: $(v).find(".img_source").last().attr("imgname"),
            ColorText: rgb2hex($(v).css("color")),
            Background: rgb2hex($(v).css("background-color")),
            FontFamily: $(v).css("font-family"),
            FontSize: $(v).css("font-size").replace('px', '')
        }

        arr.push(elm);
    });

    SendToBtn(arr);
    console.log(arr);
}

function SendToBtn(data) {
    $.ajax({
        type: "POST",
        url: "/Editor/SavaElementConfig",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location.reload(true);
        },
        failure: function (response) {
            window.location.reload(true);
        },
        error: function (response) {
            window.location.reload(true);
        }
    });
}

// Загрузка файлов
$('input[name=file_loader]').change(function (ev) {
    if (UIElementSelectPoint != null) {

        var reader = new FileReader();
        reader.onload = function (e) {
            let image = $(UIElementSelectPoint).find(".img_source").last();
            image.attr('src', e.target.result);
            image.attr('imgname', e.target.fileName);
            image[0].onload = function () {
                let elm = $("#menu-page-edit").find("input[css=width]")[0];
                let elm1 = $("#menu-page-edit").find("input[css=height]")[0];
                $(elm).val(this.width + 'px').trigger('input');
                $(elm1).val(this.height + 'px').trigger('input');
                $(this).css("width", "100%");
                $(this).css("height", "100%");
            };
        };

        let file = $("input[name=file_loader]")[0].files[0];
        reader.fileName = file.name;
        reader.readAsDataURL(file);

    }
});

// Конверт цветов
function rgb2hex(rgb) {
    var rgb = rgb.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);

    return (rgb && rgb.length === 4) ? "#" +
        ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2) : '';
}


interact('.resize-drag')
    .draggable({
        onmove: window.dragMoveListener,
        inertia: false,
                    
        restrict: {
            restriction: 'parent',
            elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
        },
    })
    .resizable({
        // resize from all edges and corners
        edges: { left: true, right: true, bottom: true, top: true },
        
        // keep the edges inside the parent
        restrictEdges: {
            outer: 'parent',
            endOnly: true,
        },
        snap: {
            targets: [
                interact.createSnapGrid({
                    x: 15,
                    y: 15
                })
            ],
            range: Infinity,
            relativePoints: [{
                x: 0,
                y: 0
            }]
        },
        // minimum size
        restrictSize: {
            min: { width: 100, height: 50 },
        },
        
        inertia: false,
    })
    .on('resizemove', function (event) {
        var target = event.target,
            x = (parseFloat(target.getAttribute('data-x')) || 0),
            y = (parseFloat(target.getAttribute('data-y')) || 0);
        
        // update the element's style
        target.style.width = event.rect.width + 'px';
        target.style.height = event.rect.height + 'px';
        ResizeDragClick(target);
        // translate when resizing from top or left edges
        x += event.deltaRect.left;
        y += event.deltaRect.top;
        
        target.style.webkitTransform = target.style.transform =
            'translate(' + x + 'px,' + y + 'px)';
        
        target.setAttribute('data-x', x);
        target.setAttribute('data-y', y);
    });

function dragMoveListener (event) {
    var target = event.target,
        x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
        y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
    target.style.transform =
    'translate(' + x + 'px, ' + y + 'px)';

    // update the posiion attributes
    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);
    ResizeDragClick(target);
}

window.dragMoveListener = dragMoveListener;
