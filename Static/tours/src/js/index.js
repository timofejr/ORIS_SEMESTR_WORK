let hotels = null;

document.addEventListener('DOMContentLoaded', function() {
    const checkinInput = document.querySelector('.header__input--checkin');
    
    checkinInput.addEventListener('click', function() {
        this.showPicker();
    });
    
    checkinInput.addEventListener('focus', function() {
        this.showPicker();
    });


    checkinInput.valueAsDate = new Date();

    const checkoutInput = document.querySelector('.header__input--checkout');
    
    checkoutInput.addEventListener('click', function() {
        this.showPicker();
    });
    
    checkoutInput.addEventListener('focus', function() {
        this.showPicker();
    });
    
    checkoutInput.valueAsDate = new Date();

    checkinInput.addEventListener('change', () => {
        if (checkinInput.value) {
        const date = new Date(checkinInput.value);
        date.setDate(date.getDate() + 1);
        checkoutInput.min = date.toISOString().split('T')[0];

        if (checkoutInput.value && checkoutInput.value < checkoutInput.min) {
            checkoutInput.value = '';
        }
        } else {
            checkoutInput.removeAttribute('min');
        }
    });

    const rangeMin = document.getElementById('rangeMin');
    const rangeMax = document.getElementById('rangeMax');
    const sliderRange = document.getElementById('slider-range');
    const minValue = document.getElementById('minValue');
    const maxValue = document.getElementById('maxValue');

    function updateSlider() {
    const min = parseInt(rangeMin.value);
    const max = parseInt(rangeMax.value);

    if (min > max - 5) rangeMin.value = max - 5;
    if (max < min + 5) rangeMax.value = min + 5;

    const percent1 = (rangeMin.value / rangeMin.max) * 100;
    const percent2 = (rangeMax.value / rangeMax.max) * 100;

    sliderRange.style.left = percent1 + '%';
    sliderRange.style.width = (percent2 - percent1) + '%';

    minValue.textContent = rangeMin.value;
    maxValue.textContent = rangeMax.value;
    }

    rangeMin.addEventListener('input', updateSlider);
    rangeMax.addEventListener('input', updateSlider);
    updateSlider();
});

document.addEventListener('DOMContentLoaded', () => {
    const button = document.getElementById('getHotels');
    button.addEventListener('click', getHotelsByResort);
    
    const resetFiltersButton = document.getElementById('reset_filters');
    resetFiltersButton.addEventListener('click', uncheckFilters);
    
    const applyFiltersButton = document.getElementById('apply_filters');
    applyFiltersButton.addEventListener('click', getHotelsByResort)
});
async function getHotelsByResort() {
    const resortInput = document.getElementById('resort');
    const resortName = resortInput.value;
    const cardsContainer = document.getElementById('cards-container');

    const date1 = document.getElementById('checkin').value;
    const date2 = document.getElementById('checkout').value;

    const d1 = new Date(date1);
    const d2 = new Date(date2);

    const diffTime = Math.abs(d2 - d1);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    const priceFrom = document.getElementById('rangeMin').value;
    const priceTo = document.getElementById('rangeMax').value;
    
    const guestsAmount = document.getElementById('guests').value;
    
    const categoriesDiv = document.getElementById('category')
    const categories = [...categoriesDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);
    
    const foodTypeDiv = document.getElementById('food_type');
    const foodTypes = [...foodTypeDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);
    
    const conceptionDiv = document.getElementById('conception')
    const conceptions = [...conceptionDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);

    const beachDistanceDiv = document.getElementById('beach_distance')
    const beachDistances = [...beachDistanceDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);
    
    const beachTypeDiv = document.getElementById('beach_type');
    const beachTypes = [...beachTypeDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);

    const roomTypeDiv = document.getElementById('room_type');
    const roomTypes = [...roomTypeDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);
    
    const servicesDiv = document.getElementById('services');
    const services = [...servicesDiv
        .querySelectorAll('input[type="checkbox"]:checked')]
        .map(cb => cb.value);
    
    const sortSelectValue = document.getElementById('sort').value;


    console.log(resortName);

    if (!resortName) {
        return;
    }

    const dataToSend = {
        ResortName: resortName,
        DaysAmount: parseInt(diffDays),
        Categories: categories,
        FoodTypes: foodTypes,
        PriceFrom: parseInt(priceFrom),
        PriceTo: parseInt(priceTo),
        BeachTypes: beachTypes,
        RoomTypes: roomTypes,
        Conceptions: conceptions,
        BeachDistances: beachDistances,
        Services: services,
        Sort: sortSelectValue,
        HotelServices: []
    };
    console.log(dataToSend)

    try {

        const response = await fetch('http://localhost:54321/getHotels/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataToSend)
        });

        if (!response.ok) {
            throw new Error(`HTTP-ошибка: Статус ${response.status}`);
        }

        hotels = await response.json();
        
        let allCards = '';
        
        let category = '';

        const toursAmount = hotels.length;

        allCards += `
                <div class="main__content__info">
                    <h3 class="main__content__title">Предложений:</h3>
                    <span id="tours_amount" class="main__content__text">${ toursAmount }</span>
                </div>
        `;
        
        
        hotels.forEach(hotel => {
            if (hotel.Category == "5 звезд"){
                category = `                        
                        <div class="main__content__product-card__hotel-five-stars">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                        </div>`;
            } else if (hotel.Category == "4 звезды") {
                category = `                        
                        <div class="main__content__product-card__hotel-four-stars">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                        </div>`;
            } else if (hotel.Category == "3 звезды") {
                category = `                        
                        <div class="main__content__product-card__hotel-three-stars">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                        </div>`;
            } else if (hotel.Category == "2 звезды") {
                category = `
                        <div class="main__content__product-card__hotel-two-stars">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                            <img class="main__content__product-card__star" src="src/img/star.svg" alt="">
                        </div>`;
            } else {
                category = `
                    <h3 class="main__content__product-card__category">${hotel.Category}</h3>
                `;
            }
            
           const card = `
                <div class="main__content__product-card">
                    <img class="main__content__product-card__left-side" src="${hotel.PhotoPath}"></img>
                    <div class="main__content__product-card__right-side">
                        <div class="main__content__product-card__location">
                            <img class="main__content__product-card__location__image" src="src/img/location.png">
                            <p class="main__content__product-card__location__text">${hotel.Location}</p>
                        </div>
                        <h3 class="main__content__product-card__hotel-name">${hotel.Name}</h3>      
                        
                        ${category}
                        <h3 class="main__content__product-card__hotel-total-price">${parseInt(hotel.Price)}₽</h3>
                        <p class="main__content__product-card__hotel-total-price-text">Цена за ${diffDays} ночей</p>
                        <button data-id="${hotel.HotelId}" class="main__content__product-card__book">Выбрать</button>
                    </div>
                </div>
           `; 
           allCards += card;
        });
        
        cardsContainer.innerHTML = allCards;

        document.querySelectorAll('.main__content__product-card__book').forEach(btn => {
            btn.addEventListener('click', () => {
                const id = btn.dataset.id;

                const resort = document.getElementById('resort').value;
                const checkin = document.getElementById('checkin').value;
                const checkout = document.getElementById('checkout').value;
                const guests = document.getElementById('guests').value;

                const url = `/${id}?&resort=${encodeURIComponent(resort)}`
                    + `&checkin=${checkin}`
                    + `&checkout=${checkout}`
                    + `&guests=${guests}`;

                window.open(url, '_blank');
            });
        });

        console.log('Ответ сервера:', hotels);

    } catch (error) {
        console.error('Ошибка:', error);
    }
}

async function uncheckFilters(){
    const checkboxes = document.querySelectorAll('#filters input[type="checkbox"]');
    
    checkboxes.forEach(cb => cb.checked = false);
}