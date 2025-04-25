// Member scripts

// Member more

function showMemberMoreModal(icon) {
    hideMemberMoreModal();

    const container = icon.closest(".members-container"); // Find the closest parent container
    const modal = container.querySelector(".member-more-modal"); // Find the modal within the container
    if (modal) {
        modal.style.display = "flex";

        // Add event listener to close modal when clicking outside
        document.addEventListener("click", closeMemberMoreModal);
    }
}

function hideMemberMoreModal() {
    const modals = document.querySelectorAll(".member-more-modal");
    modals.forEach((modal) => {
        modal.style.display = "none";
    });

    // Remove the event listener when modal is closed
    document.removeEventListener("click", closeMemberMoreModal);
}

function closeMemberMoreModal(event) {
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

const editMemberForm = document.querySelector(".edit-member-form");
if (editMemberForm) {
    editMemberForm.addEventListener("submit", async (event) => {
        const form = event.target;
        const formData = new FormData(form);

        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
            });

            if (response.ok) {
                const html = await response.text();
                document.querySelector(".over-box").innerHTML = html;
            }
        } catch (error) {
            console.error("Error:", error);
        }
    });
}

// End of edit member


// Delete Member

async function showDeleteMemberModal(memberId) {
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
        return;
    }

    try {
        const response = await fetch(`/Members/DeleteMember?id=${memberId}`, {
            method: "POST",
        });

        if (response.ok) {
            location.reload();
        } else {
            alert("Failed to delete member.");
        }
    } catch (error) {
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



// Project scripts

// Project more

function showProjectMoreModal(icon) {
    hideProjectMoreModal();

    const container = icon.closest(".project-card");
    const modal = container.querySelector(".project-more-modal");
    if (modal) {
        modal.style.display = "flex";

        document.addEventListener("click", closeProjectMoreModal);
    }
}

function hideProjectMoreModal() {
    const modals = document.querySelectorAll(".project-more-modal");
    modals.forEach((modal) => {
        modal.style.display = "none";
    });

    // Remove the event listener when modal is closed
    document.removeEventListener("click", closeProjectMoreModal);
}

function closeProjectMoreModal(event) {
    const modals = document.querySelectorAll(".project-more-modal");
    modals.forEach((modal) => {
        if (!modal.contains(event.target) && !event.target.classList.contains("more")) {
            modal.style.display = "none";
        }
    });
}

// Add project
function showAddProjectModal() {
    const modal = document.getElementById("add-project-modal");
    if (modal) {
        modal.style.display = "flex";
    }
}

function hideAddProjectModal() {
    const modal = document.getElementById("add-project-modal");
    if (modal) {
        modal.style.display = "none";

        const form = modal.querySelector(".add-project-form");
        if (form) {
            form.reset();
        }
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "project-close-button") {
        hideAddProjectModal();
    }
});

// Population of Selected members container
const selectedMembers = new Set();

// Function to add a selected member to the container
function updateSelectedMembers() {
    const dropdown = document.getElementById("member-select");
    const selectedMemberId = dropdown.value;
    const selectedMemberName = dropdown.options[dropdown.selectedIndex].getAttribute('data-name');

    const container = document.querySelector(".selected-members-container");

    if (!selectedMembers.has(selectedMemberId)) {
        selectedMembers.add(selectedMemberId);

        const memberDiv = document.createElement("div");
        memberDiv.classList.add("selected-member");
        memberDiv.setAttribute("data-id", selectedMemberId);

        memberDiv.innerHTML = `

               <p>${selectedMemberName}</p>
               <button type="button" class="btn-close" onclick="removeSelectedMember('${selectedMemberId}')"><i class="fa-solid fa-times close"></i></button>
               <input type="hidden" name="Members" value="${selectedMemberId}" />
           `;

        container.appendChild(memberDiv);
    }
}

//add this back in when image functionality works
//<img class="member-icon" src="" alt="Member Image" />

// Function to remove a selected member from the container
function removeSelectedMember(memberId) {
    selectedMembers.delete(memberId);

    const memberDiv = document.querySelector(`.selected-member[data-id="${memberId}"]`);

    if (memberDiv) {
        memberDiv.remove();
    }
}


// Add hidden inputs for selected members before form submission
const addProjectForm = document.querySelector(".add-project-form");
if (addProjectForm) {
    addProjectForm.addEventListener("submit", (event) => {
        const form = event.target;

        form.querySelectorAll("input[name='Members']").forEach(input => {
            input.remove();
        });

        // Add hidden inputs for each selected member
        selectedMembers.forEach(memberId => {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = "Members";
            input.value = memberId;
            form.appendChild(input);
        });
    });
}

// End of add project

// End of project scripts



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
