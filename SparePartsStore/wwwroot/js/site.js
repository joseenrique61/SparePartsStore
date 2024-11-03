// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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