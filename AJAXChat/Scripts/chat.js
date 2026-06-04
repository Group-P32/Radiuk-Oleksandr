$(document).ready(function () {

    // ============================
    // Логін користувача
    // ============================
    $("#btnLogin").click(function () {
        var nickName = $.trim($("#txtUserName").val());
        if (nickName) {
            var href = "/Home?user=" + encodeURIComponent(nickName);
            href = href + "&logOn=true";
            $("#LoginButton").attr("href", href).click();
            // Зберігаємо нік у блоці Username
            $("#Username").text(nickName);
        } else {
            showError("Будь ласка, введіть ім'я!");
        }
    });

    // Натискання Enter у полі імені
    $("#txtUserName").keydown(function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            $("#btnLogin").click();
        }
    });

});

// ============================
// Успішний вхід у чат
// ============================
function LoginOnSuccess(result) {
    Scroll();
    ShowLastRefresh();

    // Оновлюємо чат кожні 5 секунд
    setTimeout("Refresh();", 5000);

    // Надсилання повідомлення по Enter
    $("#txtMessage").keydown(function (e) {
        if (e.keyCode === 13 && !e.shiftKey) {
            e.preventDefault();
            $("#btnMessage").click();
        }
    });

    // Кнопка "Надіслати"
    $("#btnMessage").click(function () {
        var text = $.trim($("#txtMessage").val());
        if (text) {
            var href = "/Home?user=" + encodeURIComponent($("#Username").text());
            href = href + "&chatMessage=" + encodeURIComponent(text);
            $("#ActionLink").attr("href", href).click();
            $("#txtMessage").val("");
        }
    });

    // Кнопка "Вийти"
    $("#btnLogOff").click(function () {
        var href = "/Home?user=" + encodeURIComponent($("#Username").text());
        href = href + "&logOff=true";
        $("#ActionLink").attr("href", href).click();
        // Невелика затримка щоб запит встиг пройти
        setTimeout(function () {
            document.location.href = "/Home";
        }, 300);
    });
}

// ============================
// Помилка при логіні
// ============================
function LoginOnFailure(result) {
    $("#Username").val("");
    showError(result.responseText);
}

// ============================
// Оновлення чату кожні 5 секунд
// ============================
function Refresh() {
    var user = $("#Username").text();
    if (user) {
        var href = "/Home?user=" + encodeURIComponent(user);
        $("#ActionLink").attr("href", href).click();
        setTimeout("Refresh();", 5000);
    }
}

// ============================
// Помилка при роботі в чаті
// ============================
function ChatOnFailure(result) {
    showError(result.responseText);
}

// ============================
// Успішне оновлення чату
// ============================
function ChatOnSuccess(result) {
    Scroll();
    ShowLastRefresh();
}

// ============================
// Автоскрол до останнього повідомлення
// ============================
function Scroll() {
    var win = $("#Messages");
    if (win.length) {
        win.scrollTop(win[0].scrollHeight);
    }
}

// ============================
// Відображення часу останнього оновлення
// ============================
function ShowLastRefresh() {
    var dt = new Date();
    var h = String(dt.getHours()).padStart(2, "0");
    var m = String(dt.getMinutes()).padStart(2, "0");
    var s = String(dt.getSeconds()).padStart(2, "0");
    $("#LastRefresh").text("Останнє оновлення: " + h + ":" + m + ":" + s);
}

// ============================
// Показати помилку на 3 секунди
// ============================
function showError(msg) {
    $("#Error").text(msg);
    setTimeout(function () { $("#Error").empty(); }, 3000);
}
