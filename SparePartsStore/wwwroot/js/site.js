$(document).ready(function () {
    const filterByCategory = document.getElementById('filterByCategory')
    if (filterByCategory) {
        filterByCategory.addEventListener('change', (event) => {
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
    }

    const filterByCategoryCard = document.getElementById("filterByCategoryCard")
    if (filterByCategoryCard) {
        filterByCategoryCard.addEventListener("change", function () {
            var selectedCategory = this.value;
            var cards = document.querySelectorAll(".card");

            cards.forEach(function (card) {
                if (selectedCategory === "" || card.getAttribute("data-category-id") === selectedCategory) {
                    card.parentElement.style.display = "block";
                } else {
                    card.parentElement.style.display = "none";
                }
            });
        });
    }

    const modal1 = document.getElementById('modal1')
    if (modal1) {
        modal1.addEventListener('show.bs.modal', event => {
            const button = event.relatedTarget

            const id = button.getAttribute('data-bs-id')

            const deleteForm = document.getElementById("deleteForm")
            deleteForm.setAttribute("action", deleteForm.getAttribute("action") + "/" + id)
        })
    }

    const imgInput = document.getElementById("imgInput")
    if (imgInput) {
        imgInput.addEventListener("change", event => {
            var selectedFile = event.target.files[0];
            var reader = new FileReader();

            var imgFrame = document.getElementById("imgFrame");
            reader.onload = function (event) {
                imgFrame.src = event.target.result;
            };

            reader.readAsDataURL(selectedFile);
        })
    }
})

function AddToCart() {
    const amount = parseInt(document.getElementById("amount").innerText)

    const addForm = document.getElementById("addForm")

    addForm.setAttribute("action", addForm.getAttribute("action") + "?amount=" + amount);
}

function IncreaseAmount(max) {
    const increaseButton = document.getElementById("increaseButton")
    const decreaseButton = document.getElementById("decreaseButton")
    const buyButton = document.getElementById("addButton")
    let amount = parseInt(document.getElementById("amount").innerText)

    amount += 1

    decreaseButton.classList.remove("disabled")
    buyButton.classList.remove("disabled")
    if (amount >= max) {
        increaseButton.classList.add("disabled")
    }

    document.getElementById("amount").innerText = amount
}

function DecreaseAmount() {
    const increaseButton = document.getElementById("increaseButton")
    const decreaseButton = document.getElementById("decreaseButton")
    const buyButton = document.getElementById("addButton")
    let amount = parseInt(document.getElementById("amount").innerText)

    amount -= 1

    increaseButton.classList.remove("disabled")
    if (amount <= 0) {
        decreaseButton.classList.add("disabled")
        buyButton.classList.add("disabled")
    }

    document.getElementById("amount").innerText = amount
}
