// Member more modal

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

// End of member more modal



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
// End of add member modal


// Edit member modal

async function showEditMemberModal(memberId) {
    const modal = document.getElementById("edit-member-modal");
    console.log("Modal Found:", modal); // Check if the modal is found

    if (modal) {
        const member = await getMemberDataById(memberId);
        console.log("Member Data:", member); // Check the data returned from the server

        if (member) {
            // Debug each field being populated
            const firstNameInput = modal.querySelector("#first-name");
            console.log("First Name Input Found:", firstNameInput);
            if (firstNameInput) {
                firstNameInput.value = member.firstName;
                console.log("First Name Set:", member.firstName);
            }

            const lastNameInput = modal.querySelector("#last-name");
            console.log("Last Name Input Found:", lastNameInput);
            if (lastNameInput) {
                lastNameInput.value = member.lastName;
                console.log("Last Name Set:", member.lastName);
            }

            const emailInput = modal.querySelector("#member-email");
            console.log("Email Input Found:", emailInput);
            if (emailInput) {
                emailInput.value = member.email;
                console.log("Email Set:", member.email);
            }

            const phoneInput = modal.querySelector("#member-phone");
            console.log("Phone Input Found:", phoneInput);
            if (phoneInput) {
                phoneInput.value = member.phoneNumber; // Ensure this matches the API response
                console.log("Phone Number Set:", member.phoneNumber);
            }

            const jobInput = modal.querySelector("#member-job");
            console.log("Job Input Found:", jobInput);
            if (jobInput) {
                jobInput.value = member.jobTitle; // Ensure this matches the API response
                console.log("Job Title Set:", member.jobTitle);
            }

            const addressInput = modal.querySelector("#member-address");
            console.log("Address Input Found:", addressInput);
            if (addressInput) {
                addressInput.value = member.address;
                console.log("Address Set:", member.address);
            }

            // Debug date of birth fields
            console.log("Raw Date of Birth:", member.dateOfBirth);
            const [year, month, day] = member.dateOfBirth.split("-");
            console.log("Date of Birth Split:", { year, month, day });

            const dayInput = modal.querySelector("#member-day");
            console.log("Day Input Found:", dayInput);
            if (dayInput) {
                dayInput.value = day;
                console.log("Day Set:", day);
            }

            const monthInput = modal.querySelector("#member-month");
            console.log("Month Input Found:", monthInput);
            if (monthInput) {
                monthInput.value = month;
                console.log("Month Set:", month);
            }

            const yearInput = modal.querySelector("#member-year");
            console.log("Year Input Found:", yearInput);
            if (yearInput) {
                yearInput.value = year;
                console.log("Year Set:", year);
            }

            const dateOfBirthInput = modal.querySelector("#date-of-birth");
            console.log("Date of Birth Input Found:", dateOfBirthInput);
            if (dateOfBirthInput) {
                dateOfBirthInput.value = member.dateOfBirth;
                console.log("Date of Birth Set:", member.dateOfBirth);
            }

            modal.style.display = "flex";
            console.log("Modal Displayed");
        } else {
            console.error("Member data is null or undefined.");
        }
    } else {
        console.error("Modal with id 'edit-member-modal' not found.");
    }
}

async function getMemberDataById(memberId) {
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
    event.preventDefault();

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

// End of edit member modal


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
