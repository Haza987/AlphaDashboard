// Sidebar scripts

document.addEventListener("DOMContentLoaded", () => {
    const sidebarItems = document.querySelectorAll(".menu .item-box a");
    const currentPath = window.location.pathname.toLowerCase();

    sidebarItems.forEach((item) => {
        const href = item.getAttribute("href").toLowerCase();
        if (currentPath.includes(href)) {
            item.closest("li").classList.add("active");
        } else {
            item.closest("li").classList.remove("active");
        }
    });
});

// End of sidebar scripts



// Member scripts

// Member more

function showMemberMoreModal(icon) {
    const container = icon.closest(".members-container"); // Find the closest parent container
    const modal = container.querySelector(".member-more-modal"); // Find the modal within the container
    if (modal) {
        modal.style.display = "flex";

        // Add event listener to close modal when clicking outside
        document.addEventListener("click", closeModal);
    }
}

function hideMemberMoreModal() {
    const modals = document.querySelectorAll(".member-more-modal");
    modals.forEach((modal) => {
        modal.style.display = "none";
    });

    // Remove the event listener when modal is closed
    document.removeEventListener("click", closeModal);
}

function closeModal(event) {
    const modals = document.querySelectorAll(".member-more-modal");
    modals.forEach((modal) => {
        if (!modal.contains(event.target) && !event.target.classList.contains("more")) {
            modal.style.display = "none";
        }
    });
}

// End of member more


// Add member
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

// End of add member


// Edit member

async function showEditMemberModal(memberId) {
    const modal = document.getElementById("edit-member-modal");

    if (modal) {
        const member = await getMemberDataById(memberId);

        if (member) {
            const idInput = modal.querySelector("input[name='id']");
            if (idInput) {
                idInput.value = memberId;
            }

            const firstNameInput = modal.querySelector("#first-name");
            if (firstNameInput) {
                firstNameInput.value = member.firstName;
            }

            const lastNameInput = modal.querySelector("#last-name");
            if (lastNameInput) {
                lastNameInput.value = member.lastName;
            }

            const emailInput = modal.querySelector("#member-email");
            if (emailInput) {
                emailInput.value = member.email;
            }

            const phoneInput = modal.querySelector("#member-phone");
            if (phoneInput) {
                phoneInput.value = member.phoneNumber;
            }

            const jobInput = modal.querySelector("#member-job");
            if (jobInput) {
                jobInput.value = member.jobTitle;
            }

            const addressInput = modal.querySelector("#member-address");
            if (addressInput) {
                addressInput.value = member.address;
            }

            const [year, month, day] = member.dateOfBirth.split("-");

            const dayInput = modal.querySelector("#member-day");
            if (dayInput) {
                dayInput.value = day;
            }

            const monthInput = modal.querySelector("#member-month");
            if (monthInput) {
                monthInput.value = month;
            }

            const yearInput = modal.querySelector("#member-year");
            if (yearInput) {
                yearInput.value = year;
            }

            const dateOfBirthInput = modal.querySelector("#date-of-birth");
            if (dateOfBirthInput) {
                dateOfBirthInput.value = member.dateOfBirth;
            }

            modal.style.display = "flex";
        }
    }
}

async function getMemberDataById(memberId) {
    console.log("Fetching member data for ID:", memberId);
    const response = await fetch(`/Members/GetMemberById?id=${memberId}`);
    return await response.json();
}

function hideEditMemberModal() {
    const modal = document.getElementById("edit-member-modal");
    if (modal) {
        modal.style.display = "none";

        const form = modal.querySelector(".edit-member-form");
        if (form) {
            form.reset();
        }
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button") {

        hideEditMemberModal();
    }
});

document.querySelector(".edit-member-form").addEventListener("submit", async (event) => {
    //event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: formData,
        });

        if (response.ok) {
            const html = await response.text();
            document.querySelector(".over-box").innerHTML = html; // Update the modal content
        } else {
            console.error("Failed to update member.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
});

// End of edit member


// Delete Member

async function showDeleteMemberModal(memberId) {
    console.log("Member ID passed to modal:", memberId);
    const modal = document.getElementById("delete-member-modal");
    if (modal) {
        const member = await getMemberDataById(memberId);

        if (member) {
            const id = modal.querySelector("input[name='id']");
            if (id) {
                id.value = memberId;
            }

            const nameElement = modal.querySelector("#member-name");
            if (nameElement) {
                nameElement.textContent = `${member.firstName} ${member.lastName}`;
            }
        }
        modal.style.display = "flex";
    }
}

async function confirmDelete() {
    const modal = document.getElementById("delete-member-modal");
    const id = modal.querySelector("input[name='id']");
    const memberId = id ? id.value : null;

    if (!memberId) {
        console.error("Member ID is missing.");
        return;
    }

    try {
        const response = await fetch(`/Members/DeleteMember?id=${memberId}`, {
            method: "POST",
        });

        if (response.ok) {
            location.reload();
        } else {
            console.error("Failed to delete member.");
            alert("Failed to delete member.");
        }
    } catch (error) {
        console.error("Error deleting member:", error);
        alert("An error occurred while deleting the member.");
    }
}
function hideDeleteMemberModal() {
    const modal = document.getElementById("delete-member-modal");
    if (modal) {
        modal.style.display = "none";
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button" || event.target.id === "btn-cancel") {

        hideDeleteMemberModal();
    }
});
// End of delete member


// End of member scripts