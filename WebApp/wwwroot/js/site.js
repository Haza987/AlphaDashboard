// Add member modal
function showAddMemberModal() {
    const modal = document.getElementById("add-member-modal");
    if (modal) {
        modal.style.display = "flex";
    }
}

function hideAddMemberModal() {
    const modal = document.getElementById("add-member-modal");
    if (modal) {
        modal.style.display = "none";

        const form = modal.querySelector(".add-member-form");
        if (form) {
            form.reset();
        }
    }
}

// Event delegation for the close button
document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button") {
        
        hideAddMemberModal();
    }
});

// Add date of birth logic
function addDateOfBirth() {
    const day = document.getElementById("member-day").value;
    const month = document.getElementById("member-month").value;
    const year = document.getElementById("member-year").value;

    if (day && month && year) {
        const formattedDate = `${day.padStart(2, '0')}/${month.padStart(2, '0')}/${year}`;
        document.getElementById("date-of-birth").value = formattedDate;
    }
}
