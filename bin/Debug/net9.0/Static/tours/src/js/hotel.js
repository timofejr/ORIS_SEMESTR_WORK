const params = new URLSearchParams(window.location.search);

const resort = params.get('resort');
const checkin = params.get('checkin');
const checkout = params.get('checkout');
const guests = params.get('guests');

document.addEventListener('DOMContentLoaded', () => {

    if (document.getElementById('resort')) {
        document.getElementById('resort').value = resort || "";
    }
    if (document.getElementById('checkin')) {
        document.getElementById('checkin').value = checkin || "";
    }
    if (document.getElementById('checkout')) {
        document.getElementById('checkout').value = checkout || "";
    }
    if (document.getElementById('guests')) {
        document.getElementById('guests').value = guests || "";
    }

    let days = 1;

    if (checkin && checkout) {
        const dateIn = new Date(checkin);
        const dateOut = new Date(checkout);

        const diffMs = dateOut - dateIn;
        const diffDays = diffMs / (1000 * 60 * 60 * 24);

        if (diffDays > 0) {
            days = diffDays;
        }
    }

    const priceElems = document.querySelectorAll(".main__content__product-card__hotel-total-price");

    priceElems.forEach(elem => {
        let value = Number(elem.textContent);
        if (!isNaN(value)) {
            let newValue = value * days;
            elem.textContent = newValue;
            elem.textContent = `${newValue} ₽`;
        }
    });

    const priceTextElems = document.querySelectorAll(".main__content__product-card__hotel-total-price-text");

    priceTextElems.forEach(elem => {
        elem.textContent = `Цена за ${days} дн.`;
    });

});