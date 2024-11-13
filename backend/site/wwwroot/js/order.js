const addButton = document.querySelectorAll('.add-button');

addButton.forEach(button => {
    button.addEventListener('click', () => {
        const currentCount = parseInt(button.getAttribute('data-count'), 10);
        const newCount = currentCount + 1;

        const div = createButtonDiv(button, newCount);

        const itemId = button.getAttribute('data-item-id');
        const itemName = button.closest('.cell').querySelector('.desc').textContent;
        const itemPrice = parseFloat(button.closest('.cell').querySelector('.cost').textContent.replace('₽', ''));

        addToCart(itemId, itemName, itemPrice);

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

        addToCart(itemId, itemName, itemPrice);

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
    popupCost.textContent = cellData.price + "₽";
    popupName.textContent = cellData.name;
    popupDesc.textContent = cellData.desc;
    popupWeight.textContent = cellData.weight + "г";
    popup.style.display = 'block';
}

function hidePopup() {
    const popup = document.getElementById('popup');
    popup.style.display = 'none';
}

const cartItems = {};
let totalPrice = 0;

function addToCart(itemId, itemName, itemPrice) {
    if (cartItems[itemId]) {
        cartItems[itemId].count += 1;
    } else {
        cartItems[itemId] = { name: itemName, price: itemPrice, count: 1 };
    }
    updateCart();
}

function removeFromCart(itemId, itemName, itemPrice) {
    if (cartItems[itemId]) {
        cartItems[itemId].count -= 1;
        if (cartItems[itemId].count === 0) {
            delete cartItems[itemId];
        }
        updateCart();
    }
}

function updateCart() {
    const cartElement = document.getElementById('cart-items');
    cartElement.innerHTML = '';
    totalPrice = 0;

    Object.values(cartItems).forEach(item => {
        const itemElement = document.createElement('div');
        itemElement.className = 'desc';
        itemElement.innerHTML = `${item.name} ${item.count}шт. - ${item.price * item.count}₽`;
        cartElement.appendChild(itemElement);
        totalPrice += item.price * item.count;
    });

    document.getElementById('total-price').textContent = totalPrice;
}

function placeOrder() {
    const orderItems = Object.entries(cartItems).map(([id, item]) => ({
        dish_id: id,
        count: item.count,
        price: item.price * item.count,
    }));
    const order = {
        client_id: 1,  // заменить на реальный ID клиента
        restaurant_id: 1,  // заменить на реальный ID ресторана
        order_items: orderItems,
        comment: '',
        datetime: new Date().toISOString()
    };

    fetch('/restaurant/placeOrder', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(order)
    })
        .then(response => response.json())
        .then(data => alert(`Заказ создан! ID заказа: ${data.id}`))
        .catch(error => console.error('Ошибка:', error));

    console.log('Order Items:', orderItems);
    console.log('Total Price:', totalPrice);
}
