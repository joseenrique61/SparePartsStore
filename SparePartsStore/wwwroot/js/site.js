$(document).ready(function () {
    document.getElementById('filterByCategory').addEventListener('change', (event) => {
        const selectedCategory = event.target.value;
        const rows = document.querySelectorAll('#inventory tbody tr');

        rows.forEach(row => {
            const categoryId = row.getAttribute('category-id');
            if (selectedCategory === "" || categoryId === selectedCategory) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });
    });
})

$(document).ready(function () {
    document.getElementById("filterByCategoryCard").addEventListener("change", function () {
        var selectedCategory = this.value;
        var cards = document.querySelectorAll(".card");

        cards.forEach(function (card) {
            if (selectedCategory === "" || card.getAttribute("data-category-id") === selectedCategory) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    });
})


function AddToCart() {
    const amount = parseInt(document.getElementById("amount").innerText)

    const addForm = document.getElementById("addForm")

    addForm.setAttribute("action", addForm.getAttribute("action") + "?amount=" + amount);
}

function IncreaseAmount(max) {
    const increaseButton = document.getElementById("increaseButton")
    const decreaseButton = document.getElementById("decreaseButton")
    let amount = parseInt(document.getElementById("amount").innerText)

    amount += 1

    decreaseButton.classList.remove("disabled")
    if (amount >= max) {
        increaseButton.classList.add("disabled")
    }

    document.getElementById("amount").innerText = amount
}

function DecreaseAmount() {
    const increaseButton = document.getElementById("increaseButton")
    const decreaseButton = document.getElementById("decreaseButton")
    let amount = parseInt(document.getElementById("amount").innerText)

    amount -= 1

    increaseButton.classList.remove("disabled")
    if (amount <= 0) {
        decreaseButton.classList.add("disabled")
    }

    document.getElementById("amount").innerText = amount
}

