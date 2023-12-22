function selectSeat(containerElement, seatId) {
    // Check if the seat is already selected or occupied
    if (containerElement.classList.contains('seat-occupied') || containerElement.classList.contains('seat-selected')) {
        return; // Do nothing if the seat is occupied or already selected
    }
    
    // Deselect any previously selected seat
    var previouslySelected = document.querySelector('.seat-selected');
    if (previouslySelected) {
        previouslySelected.classList.remove('seat-selected');
        previouslySelected.querySelector('.seat-radio').checked = false;
    }
    
    // Select the new seat
    containerElement.classList.add('seat-selected');
    containerElement.querySelector('.seat-radio').checked = true;
}   
// seat.js dosyası içerisinde
document.addEventListener('DOMContentLoaded', function() {
    // Tüm koltuk konteynerlarını seç
    var seats = document.querySelectorAll('.seat-container');
    
    // Her koltuk konteynerı için tıklama olayı ekleyin
    seats.forEach(function(seat) {
        seat.addEventListener('click', function() {
            // selectSeat fonksiyonunu çağır
            selectSeat(this, this.querySelector('.seat-radio').value);
        });
    });
});

