const addButton = document.querySelectorAll('.add-button');

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

document.getElementById('scrollToBottomBtn').addEventListener('click', function() {
    window.scrollTo({
        top: document.body.scrollHeight,
        behavior: 'smooth'
    });
});

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

    // Привязываем обработчик события 'click' к новой кнопке
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
        showPopup(cellData);
    }
}

function showPopup(cellData) {
    const popup = document.getElementById('popup');
    const popupImage = document.getElementById('popup-image');
    const popupName = document.getElementById('popup-desc');
    const popupCost = document.getElementById('popup-cost');
    const popupDesc = document.getElementById('popup-full-desc');
    const popupWeight = document.getElementById('popup-weight');
    popupImage.src = cellData.image;
    popupCost.textContent = parseFloat(cellData.price).toFixed(2) + "₽"; // Округляем до двух знаков после запятой
    popupName.textContent = cellData.name;
    popupDesc.textContent = cellData.desc;
    popupWeight.textContent = cellData.weight + "г";
    popup.style.display = 'block';
}

function hidePopup() {
    const popup = document.getElementById('popup');
    popup.style.display = 'none';
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
        itemElement.className = 'desc';
        const totalItemPrice = (item.price * item.count).toFixed(2); // Округляем до двух знаков после запятой
        itemElement.innerHTML = `${item.name} ${item.count}шт. - ${totalItemPrice}₽`;
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
    const encoder = new TextEncoder();
    const dataBuffer = encoder.encode(text);
    const hashBuffer = await crypto.subtle.digest('SHA-256', dataBuffer);
    const hashArray = new Uint8Array(hashBuffer);
    const orderId = (hashArray[0] << 24) | (hashArray[1] << 16) | (hashArray[2] << 8) | hashArray[3];
    return orderId & 0x7FFFFFFF;
}


async function placeOrder() {
    const contextElement = document.getElementById('order-context');
    const tableId = parseInt(contextElement.getAttribute('data-table-id'), 10);
    const restaurantId = parseInt(contextElement.getAttribute('data-restaurant-id'), 10);
    const clientId = parseInt(contextElement.getAttribute('data-client-id'), 10);
    const comment = document.getElementById('input').value;

    const data = {
        tableId,
        restaurantId,
        clientId,
        cartItems: Object.entries(cartItems),
        comment: comment || "",
        timestamp: new Date().toISOString()
    };

    console.log("Input for hash:", data); 


    const orderId = await generateOrderId(data);
    console.log("orderId", orderId)
    

    const orderItems = Object.entries(cartItems).map(([id, item], index) => ({
        id: index + 1,
        menu_item_id: parseInt(id, 10),
        count: item.count,
        price: parseFloat((item.price * item.count).toFixed(2)),
        order_id: orderId
    }));

    const client = {
        Id: clientId,
        Ip: "2131232"
    };

    const order = {
        "Id": orderId,
        "client_id": clientId,
        "table_id": tableId,
        "cooking_time_minutes": cartCookingTime,
        "Comment": comment || "",
        "date_time": new Date().toISOString(),
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