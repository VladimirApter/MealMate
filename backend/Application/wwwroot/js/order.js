let currentAddButton = null;

document.addEventListener('DOMContentLoaded', () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
    } else {
        alert("Geolocation не поддерживается вашим браузером.");
        isUserInAllowedZone = false;
        initializePage();
    }
});


const restaurantLatitudeElement = document.getElementById("resPositionLatitude");
const restaurantLongitudeElement = document.getElementById("resPositionLongitude");
restaurantLatitudeElement.style.display = 'none'
restaurantLongitudeElement.style.display = 'none'
let isUserInAllowedZone = false
const restaurantLatitude = parseFloat(restaurantLatitudeElement.textContent.replace(',', '.'));
const restaurantLongitude = parseFloat(restaurantLongitudeElement.textContent.replace(',', '.'));


function successCallback(position) {
    const userLatitude = position.coords.latitude;
    const userLongitude = position.coords.longitude;
    console.log("Ресторан:", restaurantLatitude, restaurantLongitude)
    console.log("Пользователь:", userLatitude, userLongitude)

    const distance = calculateDistance(
        userLatitude,
        userLongitude,
        restaurantLatitude,
        restaurantLongitude
    );
    console.log("Distance:", distance)

    if (distance <= 0.5) {
        isUserInAllowedZone = true;
    } else {
        isUserInAllowedZone = false;
        alert("Вы находитесь слишком далеко от ресторана. Доступ к некоторым функциям ограничен.");
    }
    initializePage();
}

function errorCallback(error) {
    console.error(error);
    alert("Не удалось получить ваше местоположение. Доступ к некоторым функциям ограничен.");
    isUserInAllowedZone = false;
    initializePage();
}

function calculateDistance(lat1, lon1, lat2, lon2) {
    const R = 6371; // Радиус Земли в километрах
    const dLat = toRadians(lat2 - lat1);
    const dLon = toRadians(lon2 - lon1);
    const a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(toRadians(lat1)) *
        Math.cos(toRadians(lat2)) *
        Math.sin(dLon / 2) *
        Math.sin(dLon / 2);

    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    const distance = R * c; // Расстояние в километрах
    console.log("Расстояние до ресторана: " + (distance * 1000).toFixed(2) + " метров");

    return distance;
}

function toRadians(degrees) {
    return (degrees * Math.PI) / 180;
}

function initializePage() {
    const addButtons = document.querySelectorAll('.add-button');
    const orderButton = document.getElementById('order-button');
    const callButton = document.getElementById('call');

    if (!isUserInAllowedZone) {
        // Блокируем кнопки и добавляем обработчик для повторного запроса геолокации
        addButtons.forEach(button => {
            button.addEventListener('click', requestGeolocationPermission);
        });

        if (orderButton) {
            //orderButton.disabled = true;
            orderButton.addEventListener('click', requestGeolocationPermission)
            orderButton.style.display = 'none'
            //orderButton.addEventListener('click', requestGeolocationPermission);
        }

        if (callButton) {
            callButton.addEventListener('click', requestGeolocationPermission)
            callButton.style.display = 'none'; // Скрываем кнопку вызова официанта
        }
    } else {
        initializePage2()
    }
}

function requestGeolocationPermission(event) {
    event.preventDefault();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
        alert("Для выполнения этого действия необходимо находиться в ресторане");
    } else {
        alert("Геолокация не поддерживается вашим браузером.");
    }
}

function initializePage2(){
    const addButton = document.querySelectorAll('.add-button');
    const orderButton = document.getElementById('order-button');
    const callButton = document.getElementById('call');

    addButton.forEach(button => {
        button.addEventListener('click', () => {
            const currentCount = parseInt(button.getAttribute('data-count'), 10);
            const newCount = currentCount + 1;

            const div = createButtonDiv(button, newCount);

            const itemId = button.getAttribute('data-item-id');
            const itemName = button.closest('.cell').querySelector('.desc').textContent;
            const itemPrice = parseFloat(button.closest('.cell').querySelector('.cost').textContent.replace('₽', ''));
            const itemCookingTime = parseFloat(button.closest('.cell').querySelector('.cooking-time').textContent);

            addToCart(itemId, itemName, itemPrice, itemCookingTime);

            button.replaceWith(div);
        });
    });
    
    orderButton.addEventListener('click', placeOrder)
    callButton.addEventListener('click', callWaiter)


    document.getElementById('scrollToBottomBtn').addEventListener('click', function() {
        window.scrollTo({
            top: document.body.scrollHeight,
            behavior: 'smooth'
        });
    });
}

function createButtonDiv(button, newCount) {
    const div = document.createElement('div');
    div.className = 'add-button';
    div.setAttribute('data-item-id', button.getAttribute('data-item-id'));
    div.setAttribute('data-count', newCount);

    div.innerHTML = `
        <span class="action1" onclick="minus('${button.getAttribute('data-item-id')}')">-</span>
        <span class="text">${newCount}</span>
        <span class="action1" onclick="plus('${button.getAttribute('data-item-id')}')">+</span>`;

    return div;
}

function plus(id) {
    const button = document.querySelector(`.add-button[data-item-id="${id}"]`);
    if (button) {
        const currentCount = parseInt(button.getAttribute('data-count'), 10);
        const newCount = currentCount + 1;
        updateButtonCount(button, newCount);

        const itemId = button.getAttribute('data-item-id');
        const itemName = button.closest('.cell').querySelector('.desc').textContent;
        const itemPrice = parseFloat(button.closest('.cell').querySelector('.cost').textContent.replace('₽', ''));
        addToCart(itemId, itemName, itemPrice);
    }
}

function minus(id) {
    const button = document.querySelector(`.add-button[data-item-id="${id}"]`);
    if (button) {
        const currentCount = parseInt(button.getAttribute('data-count'), 10);
        const newCount = currentCount - 1;
        const itemId = button.getAttribute('data-item-id');
        const itemName = button.closest('.cell').querySelector('.desc').textContent;
        const itemPrice = parseFloat(button.closest('.cell').querySelector('.cost').textContent.replace('₽', ''));

        if (newCount === 0) {
            const originalButton = createOriginalButton(button);
            button.replaceWith(originalButton);
            removeFromCart(itemId, itemName, itemPrice);
        } else {
            updateButtonCount(button, newCount);
            removeFromCart(itemId, itemName, itemPrice);
        }
    }
}


function createOriginalButton(button) {
    const originalButton = document.createElement('button');
    originalButton.className = 'add-button'; // или другие необходимые классы
    originalButton.setAttribute('data-item-id', button.getAttribute('data-item-id'));
    originalButton.setAttribute('data-count', '0');
    originalButton.innerHTML = `
        <span class="plus">+</span>
        <span class="text">Добавить</span>
    `;

    originalButton.addEventListener('click', () => {
        const currentCount = parseInt(originalButton.getAttribute('data-count'), 10) || 0;
        const newCount = currentCount + 1;

        const div = createButtonDiv(originalButton, newCount);

        const itemId = originalButton.getAttribute('data-item-id');
        const itemName = originalButton.closest('.cell').querySelector('.desc').textContent;
        const itemPrice = parseFloat(originalButton.closest('.cell').querySelector('.cost').textContent.replace('₽', ''));
        const itemCookingTime = parseFloat(originalButton.closest('.cell').querySelector('.cooking-time').textContent);

        addToCart(itemId, itemName, itemPrice, itemCookingTime);

        originalButton.replaceWith(div);
    });

    return originalButton;
}

function updateButtonCount(button, newCount) {
    button.setAttribute('data-count', newCount);
    button.querySelector('span:nth-child(2)').textContent = newCount;
}

function handleCellClick(event, cellElement) {

    if (!event.target.closest('.add-button')) {
        const cellData = JSON.parse(cellElement.getAttribute('data-cell'));
        currentAddButton = cellElement.querySelector('.add-button');
        showPopup(cellData);
    }
}

function showPopup(cellData) {
    document.getElementById('overlay').style.display = 'block';
    document.body.style.overflow = 'hidden';
    const popup = document.getElementById('popup');
    const popupImage = document.getElementById('popup-image');
    const popupName = document.getElementById('popup-desc');
    const popupCost = document.getElementById('popup-cost');
    const popupDesc = document.getElementById('popup-full-desc');
    const popupCtm = document.getElementById('popup-ctm');

    console.log(cellData)
    if (cellData.image !== null) {
        popupImage.src = "/MenuItemImages/" + cellData.image;
    }
    else {
        popupImage.src = "https://i.ebayimg.com/images/g/lbUAAOSw6DtmH8z0/s-l1600.png";
    }
    popupCost.textContent = parseFloat(cellData.price).toFixed(2) + "₽";
    popupName.textContent = cellData.name;
    popupDesc.textContent = cellData.desc;
    
    proteins = document.getElementById('proteins');
    if (cellData.nutrients.Proteins !== null) {
        proteins.textContent = cellData.nutrients.Proteins + "г";
    } else {
        proteins.textContent = "-";
    }

    fats = document.getElementById('fats');
    if (cellData.nutrients.Fats !== null) {
        fats.textContent = cellData.nutrients.Fats + "г";
    } else {
        fats.textContent = "-";
    }

    carbohydrates = document.getElementById('carbohydrates');
    if (cellData.nutrients.Carbohydrates !== null) {
        carbohydrates.textContent = cellData.nutrients.Carbohydrates + "г";
    } else{
        carbohydrates.textContent = "-";
    }

    kkal = document.getElementById('kkal');
    if (cellData.nutrients.Kilocalories !== null) {
        kkal.textContent = cellData.nutrients.Kilocalories;
    } else {
        kkal.textContent = "-";
    }

    popupCtm.textContent = "Время приготовления: " + cellData.cooking_time + " минут";
    popup.style.display = 'block';
}

function hidePopup() {
    const popup = document.getElementById('popup');
    popup.style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
    document.body.style.overflow = '';
}

let cartItems = {};
let totalPrice = 0.0
let cartCookingTime = 0.0
function addToCart(itemId, itemName, itemPrice, itemCookingTime = 0) {
    if (cartItems[itemId]) {
        cartItems[itemId].count += 1;
    } else {
        cartItems[itemId] = { name: itemName, price: parseFloat(itemPrice).toFixed(2), count: 1, itemCookingTime};
    }
    updateCart();
}


function removeFromCart(itemId, itemName, itemPrice) {
    if (cartItems[itemId]) {
        cartItems[itemId].count -= 1;
        if (cartItems[itemId].count === 0) {
            delete cartItems[itemId];
        }
    }
    updateCart();
}


function updateCart() {
    const cartElement = document.getElementById('cart-items');
    cartElement.innerHTML = '';
    totalPrice = 0.0;

    let nowMaxTime = 0

    Object.values(cartItems).forEach(item => {
        const itemElement = document.createElement('div');
        itemElement.className = 'desc bg';
        const totalItemPrice = (item.price * item.count).toFixed(2); // Округляем до двух знаков после запятой
        itemElement.innerHTML = item.name + " " + item.count + "шт." + " " + totalItemPrice + "₽";
        cartElement.appendChild(itemElement);
        totalPrice += parseFloat(totalItemPrice);
        if(item.itemCookingTime > nowMaxTime){
            nowMaxTime = item.itemCookingTime
        }
    });

    cartCookingTime = nowMaxTime;

    document.getElementById('total-price').textContent = totalPrice.toFixed(2);// Округляем общую сумму до двух знаков после запятой
    document.getElementById('total-time').textContent = cartCookingTime;
}


async function generateOrderId(data) {
    const text = JSON.stringify(data);
    const hash = CryptoJS.SHA256(text);
    const orderId = hash.words[0] & 0x7FFFFFFF;
    return orderId;
}

function getLocalISOTime() {
    let tzoffset = (new Date()).getTimezoneOffset() * 60000;
    let localISOTime = (new Date(Date.now() - tzoffset)).toISOString();
    return localISOTime;
}

async function placeOrder() {
    const contextElement = document.getElementById('order-context');
    const tableId = parseInt(contextElement.getAttribute('data-table-id'), 10);
    const restaurantId = parseInt(contextElement.getAttribute('data-restaurant-id'), 10);
    const clientId = parseInt(contextElement.getAttribute('data-client-id'), 10);
    const comment = document.getElementById('input').value;
    
    if (Object.keys(cartItems).length === 0){
        alert('Ваша корзина пуста!');
        return;
    }    

    const data = {
        tableId,
        restaurantId,
        clientId,
        cartItems: Object.entries(cartItems),
        comment: comment || "",
        timestamp: getLocalISOTime(),
    };

    console.log("Input for hash:", data);


    const orderId = await generateOrderId(data);
    console.log("orderId", orderId)


    const orderItems = Object.entries(cartItems).map(([id, item], index) => ({
        id: index + 1,
        menu_item_id: parseInt(id, 10),
        count: item.count,
        price: parseFloat((item.price * item.count).toFixed(2)),
        order_id: orderId,
    }));

    const client = {
        Id: clientId,
        Ip: "zaglushka"
    };

    const order = {
        "Id": orderId,
        "client_id": clientId,
        "table_id": tableId,
        "cooking_time_minutes": cartCookingTime,
        "Comment": comment || "",
        "date_time": getLocalISOTime(),
        "Status": 0,
        "Client": client,
        "order_items": orderItems,
        "Price": totalPrice
    };

    console.log('Order', order);

    try {
        const response = await fetch('/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(order),
        });

        if (response.ok) {
            clearCart();
            const data = await response.json();
            console.log('Response from server:', data); // Лог для проверки
            const orderUrl = data.url;
            if (orderUrl) {
                clearCart();
                if (window.location.href.at(-1) === '/') {
                    window.location.href += orderUrl;
                } else {
                    window.location.href += '/' + orderUrl;
                }
            }} else {
            alert('Ошибка при отправке заказа');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка при отправке заказа');
    }
}

function clearCart() {
    cartItems = {};
    updateCart();
}

document.getElementById('waiter').addEventListener('click', callWaiter);

async function callWaiter() {
    const contextElement = document.getElementById('order-context');
    const tableId = parseInt(contextElement.getAttribute('data-table-id'), 10);
    const clientId = parseInt(contextElement.getAttribute('data-client-id'), 10);
    
    const client = {
        Id: clientId,
        Ip: "zaglushka"
    };

    const waiterCall = {
        "client_id": clientId,
        "table_id": tableId,
        "date_time": getLocalISOTime(),
        "status": 0,
        "client": client
    };

    try {
        const response = await fetch('/waitercall', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(waiterCall),
        });

        if (response.ok) {
            const textElement = document.getElementById('callText');
            const originalText = textElement.textContent;
            const callBlock = document.getElementById('call');
            const originalOnClick = callBlock.getAttribute('onclick');
            callBlock.style.backgroundColor = "#EF8A38";
            textElement.textContent = 'Официант уже идет';
            callBlock.setAttribute('onclick', '');
            setTimeout(() => {
                textElement.textContent = originalText;
                callBlock.style.backgroundColor = "#376DD9";
                callBlock.setAttribute('onclick', originalOnClick);
            }, 30000);
        } else {
            alert('Ошибка при вызове официанта.');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка при вызове официанта.');
    }
}

function handlePopupAddButtonClick() {
    hidePopup();
    if (currentAddButton) {
        console.log(currentAddButton)
        currentAddButton.click();
    }
}

