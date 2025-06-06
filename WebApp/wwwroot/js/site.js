﻿// Theme scripts

function toggleTheme() {
    const currentTheme = document.documentElement.getAttribute("data-theme") || "light";
    const newTheme = currentTheme === "light" ? "dark" : "light";

    document.documentElement.setAttribute("data-theme", newTheme);

    localStorage.setItem("theme", newTheme);
}

document.addEventListener("DOMContentLoaded", () => {
    const savedTheme = localStorage.getItem("theme") || "light";
    document.documentElement.setAttribute("data-theme", savedTheme);
});

document.addEventListener("DOMContentLoaded", () => {
    const checkbox = document.getElementById("custom-checkbox");
    const savedTheme = localStorage.getItem("theme") || "light";

    document.documentElement.setAttribute("data-theme", savedTheme);

    checkbox.checked = savedTheme === "dark";

    checkbox.addEventListener("change", toggleTheme);
});

// End of theme scripts


// Header scripts

function showSettingsModal(icon) {
    const modal = document.querySelector(".user-container");

    if (modal) {

        if (modal.style.display === "grid") {
            modal.style.display = "none";
            document.removeEventListener("click", closeSettingsModal);
        } else {
            modal.style.display = "grid";
            document.addEventListener("click", closeSettingsModal);
        }
    }
}

function hideSettingsModal() {
    const modals = document.querySelectorAll(".user-container");
    modals.forEach((modal) => {
        modal.style.display = "none";
    });
    // Remove the event listener when modal is closed
    document.removeEventListener("click", closeSettingsModal);
}

function closeSettingsModal(event) {
    const modal = document.querySelector(".user-container");
    if (modal && !modal.contains(event.target) && !event.target.classList.contains("fa-gear")) {
        modal.style.display = "none";
        document.removeEventListener("click", closeSettingsModal);
    }
}

// End of header scripts


// Member scripts

// Member more modal

function showMemberMoreModal(icon) {
    hideMemberMoreModal();

    const container = icon.closest(".members-container");
    const modal = container.querySelector(".member-more-modal");
    if (modal) {
        modal.style.display = "flex";

        document.addEventListener("click", closeMemberMoreModal);
    }
}

function hideMemberMoreModal() {
    const modals = document.querySelectorAll(".member-more-modal");
    modals.forEach((modal) => {
        modal.style.display = "none";
    });

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

// End of add member modal


// Edit member modal

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

// End of edit member modal


// Delete Member modal

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
// End of delete member modal


// Member creation form validation

function validateMember() {
    let isValid = true;

    const firstName = document.forms["createMember"]["firstName"].value;
    const firstNameError = document.getElementById("first-name-error");
    if (firstName == "") {
        firstNameError.style.display = "block";
        isValid = false;
    } else {
        firstNameError.style.display = "none";
    }

    const lastName = document.forms["createMember"]["lastName"].value;
    const lastNameError = document.getElementById("last-name-error");
    if (lastName == "") {
        lastNameError.style.display = "block";
        isValid = false;
    } else {
        lastNameError.style.display = "none";
    }

    const email = document.forms["createMember"]["email"].value;
    const emailError = document.getElementById("email-error");
    if (email == "") {
        emailError.style.display = "block";
        isValid = false;
    } else {
        emailError.style.display = "none";
    }

    const phone = document.forms["createMember"]["phoneNumber"].value;
    const phoneError = document.getElementById("phone-error");
    if (phone == "") {
        phoneError.style.display = "block";
        isValid = false;
    } else {
        phoneError.style.display = "none";
    }

    const jobTitle = document.forms["createMember"]["jobTitle"].value;
    const jobTitleError = document.getElementById("job-title-error");
    if (jobTitle == "") {
        jobTitleError.style.display = "block";
        isValid = false;
    } else {
        jobTitleError.style.display = "none";
    }

    const address = document.forms["createMember"]["address"].value;
    const addressError = document.getElementById("address-error");
    if (address == "") {
        addressError.style.display = "block";
        isValid = false;
    } else {
        addressError.style.display = "none";
    }

    const day = document.getElementById("member-day").value;
    const month = document.getElementById("member-month").value;
    const year = document.getElementById("member-year").value;
    const dateOfBirthError = document.getElementById("date-of-birth-error");
    if (day == "" || month == "" || year == "") {
        dateOfBirthError.style.display = "block";
        isValid = false;
    } else {
        dateOfBirthError.style.display = "none";
    }

    if (!isValid) {
        event.preventDefault();
    }
}

// end of member creation form validation

// End of member scripts



// Project scripts

// Project filter
function filterProjects(filter) {
    const url = new URL(window.location.href);
    url.searchParams.set('filter', filter);
    window.location.href = url.toString();
}

// End of project filter


// Project more modal

function showProjectMoreModal(icon, event) {
    event.stopPropagation(event);
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

// End of project more modal


// Project info modal

async function showProjectInfoModal(projectId) {
    const modal = document.getElementById("project-info-modal");

    if (modal) {
        const response = await fetch(`/Projects/UpdateProject?id=${projectId}`);
        if (response.ok) {
            const project = await response.json();
            modal.querySelector("#project-id-custom").textContent = project.projectId;
            modal.querySelector("#project-image").src = project.projectImageFilePath;
            modal.querySelector("#project-name").textContent = project.projectName;
            modal.querySelector("#project-client").textContent = project.clientName;
            modal.querySelector("#project-description").textContent = project.projectDescription;
            modal.querySelector("#project-start").textContent = new Date(project.startDate).toLocaleDateString("en-GB");
            modal.querySelector("#project-end").textContent = new Date(project.endDate).toLocaleDateString("en-GB");
            modal.querySelector("#project-budget").textContent = project.budget;
            modal.querySelector("#project-id-input").value = project.id;

            const membersContainer = modal.querySelector("#selected-members-info .selected-members-container");
            membersContainer.innerHTML = "";

            if (project.members && project.members.length > 0) {

                for (const memberId of project.members) {
                    const member = await getMemberDataById(memberId);
                    if (member) {
                        const memberDiv = document.createElement("div");
                        memberDiv.classList.add("selected-member");
                        memberDiv.innerHTML = `
                            <p>${member.firstName} ${member.lastName}</p>
                        `;
                        membersContainer.appendChild(memberDiv);
                    }
                }
            } else {
                membersContainer.innerHTML = "<p>No members assigned to this project.</p>";
            }

            modal.style.display = "flex";
        }
    }
}

function hideProjectInfoModal() {
    const modal = document.getElementById("project-info-modal");
    if (modal) {
        modal.style.display = "none";
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button") {
        hideProjectInfoModal();
    }
});

const projectInfoCardForm = document.querySelector(".project-info-card-form");

if (projectInfoCardForm) {
    projectInfoCardForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const form = event.target;
        const formData = new FormData(form);
        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
            });

            if (response.ok) {
                location.reload();
            } else {
                console.error("Failed to update project status.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("An error occurred while updating the project status.");
        }
    });
}

function setIsCompleted(value) {
    const isCompletedInput = document.getElementById("is-completed-input");
    if (isCompletedInput) {
        isCompletedInput.value = value;
    }
}

// End of project info modal


// Add project modal
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

        const errorMessages = modal.querySelectorAll(".project-error");
        errorMessages.forEach((error) => {
            error.style.display = "none";
        });

        const fileInput = document.querySelector("#file");
        const img = document.querySelector("#image-preview");
        const circle = document.querySelector("#circle-container");
        const previewContainer = document.querySelector(".image-preview-container");

        if (fileInput) {
            fileInput.value = "";
        }
        if (img) {
            img.src = "#";
            img.classList.add("d-none");
        }
        if (circle) {
            circle.classList.remove("d-none");
        }
        if (previewContainer) {
            previewContainer.classList.add("d-none");
        }
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "project-close-button") {
        hideAddProjectModal();
    }
});

const addProjectForm = document.querySelector(".add-project-form");
if (addProjectForm) {
    addProjectForm.addEventListener("submit", (event) => {
        const form = event.target;

        const memberError = document.getElementById("member-error");
        if (selectedMembers.size === 0) {
            memberError.style.display = "block";
            event.preventDefault();
            return;
        } else {
            memberError.style.display = "none";
        }

        form.querySelectorAll("input[name='Members']").forEach(input => {
            input.remove();
        });

        selectedMembers.forEach(memberId => {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = "Members";
            input.value = memberId;
            form.appendChild(input);
        });
    });
}

// End of add project modal


// Shared selected members container

const selectedMembers = new Set();
function updateSelectedMembers(dropdownId, containerId) {
    const dropdown = document.getElementById(dropdownId);
    const selectedMemberId = dropdown.value;
    const selectedMemberName = dropdown.options[dropdown.selectedIndex]?.getAttribute('data-name');

    // Ignore the default option
    if (selectedMemberId === "-- Assign members --" || !selectedMemberName) {
        return;
    }

    const container = document.getElementById(containerId);

    if (!selectedMembers.has(selectedMemberId)) {
        selectedMembers.add(selectedMemberId);

        const memberDiv = document.createElement("div");
        memberDiv.classList.add("selected-member");
        memberDiv.setAttribute("data-id", selectedMemberId);

        memberDiv.innerHTML = `
        <p>${selectedMemberName}</p>
        <button type="button" class="btn-close" onclick="removeSelectedMember('${selectedMemberId}', '${containerId}')">
            <i class="fa-solid fa-times close"></i>
        </button>
        <input type="hidden" name="Members" value="${selectedMemberId}" />
    `;

        container.querySelector(".selected-members-container").appendChild(memberDiv);
    }
}
function removeSelectedMember(memberId, containerId) {

    selectedMembers.delete(memberId);

    const container = document.getElementById(containerId);

    const memberDiv = container.querySelector(`.selected-member[data-id="${memberId}"]`);

    if (memberDiv) {
        memberDiv.remove();
    }

    const hiddenInput = document.querySelector(`input[name="Members"][value="${memberId}"]`);
    if (hiddenInput) {
        hiddenInput.remove();
    }
}

// End of shared section


// Edit project modal

async function showEditProjectModal(projectId, event) {
    event.stopPropagation();
    const modal = document.getElementById("edit-project-modal");

    if (modal) {
        await fetch(`/Projects/UpdateProject?id=${projectId}`);

        const project = await getProjectDataById(projectId);

        if (project) {

            const imagePreview = modal.querySelector("#project-image");
            if (project.projectImageFilePath) {
                imagePreview.src = project.projectImageFilePath;
            }

            const idInput = modal.querySelector("input[name='id']");
            if (idInput) {
                idInput.value = project.id;
            }

            const titleElement = modal.querySelector("#project-id");
            if (titleElement) {
                titleElement.textContent = `Edit Project - ${project.projectId}`;
            }

            const ProjectNameInput = modal.querySelector("#project-name");
            if (ProjectNameInput) {
                ProjectNameInput.value = project.projectName;
            }

            const ClientNameInput = modal.querySelector("#project-client");
            if (ClientNameInput) {
                ClientNameInput.value = project.clientName;
            }

            const ProjectDescriptionInput = modal.querySelector("#project-description");
            if (ProjectDescriptionInput) {
                ProjectDescriptionInput.value = project.projectDescription;
            }

            const startDateInput = modal.querySelector("#project-start");
            if (startDateInput) {
                startDateInput.value = project.startDate.split("T")[0];
            }

            const endDateInput = modal.querySelector("#project-end");
            if (endDateInput) {
                endDateInput.value = project.endDate.split("T")[0];
            }

            const membersContainer = modal.querySelector("#selected-members-container-edit .selected-members-container");
            if (membersContainer && project.members) {
                project.members.forEach(member => {
                    const memberDiv = document.createElement("div");
                    memberDiv.classList.add("selected-member");
                    memberDiv.setAttribute("data-id", member.id);

                    memberDiv.innerHTML = `
                    <p>${member.firstName} ${member.lastName}</p>
                    <button type="button" class="btn-close" onclick="removeSelectedMember('${member.id}', 'selected-members-container')">
                        <i class="fa-solid fa-times close"></i>
                    </button>
                    <input type="hidden" name="Members" value="${member.id}" />
    `;

                    membersContainer.appendChild(memberDiv);
                });
            }

            const budgetInput = modal.querySelector("#project-budget");
            if (budgetInput) {
                budgetInput.value = project.budget;
            }

            modal.style.display = "flex";
        }
    }
}

async function getProjectDataById(projectId) {
    const response = await fetch(`/Projects/GetProjectById?id=${projectId}`);
    return await response.json();
}

function hideEditProjectModal() {
    const modal = document.getElementById("edit-project-modal");
    if (modal) {
        modal.style.display = "none";
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button") {
        hideEditProjectModal();
    }
});

const editProjectForm = document.querySelector(".edit-project-form");
if (editProjectForm) {
    editProjectForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        const form = event.target;

        const formData = new FormData(form);

        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
            });

            if (response.ok) {
                location.reload();
            } 
        }
    });
}
// End of edit project modal


// Delete project modal

async function showDeleteProjectModal(projectId, event) {
    event.stopPropagation();
    const modal = document.getElementById("delete-project-modal");
    if (modal) {
        const project = await getProjectDataById(projectId);

        if (project) {
            const id = modal.querySelector("input[name='id']");
            if (id) {
                id.value = projectId;
            }

            const nameElement = modal.querySelector("#project-name");
            if (nameElement) {
                nameElement.textContent = `${project.projectName}`;
            }

            const imageElement = modal.querySelector("#project-image")
            if (imageElement) {
                imageElement.src = project.projectImageFilePath
            }
        }
        modal.style.display = "flex";
    }
}

async function confirmDelete() {
    const modal = document.getElementById("delete-project-modal");
    const id = modal.querySelector("input[name='id']");
    const projectId = id ? id.value : null;

    if (!projectId) {
        return;
    }

    try {
        const response = await fetch(`/Projects/DeleteProject?id=${projectId}`, {
            method: "POST",
        });

        if (response.ok) {
            location.reload();
        } else {
            alert("Failed to delete project.");
        }
    } catch (error) {
        alert("An error occurred while deleting the project.");
    }
}

function hideDeleteProjectModal() {
    const modal = document.getElementById("delete-project-modal");
    if (modal) {
        modal.style.display = "none";
    }
}

document.addEventListener("click", (event) => {
    if (event.target && event.target.id === "close-button" || event.target.id === "btn-cancel") {
        hideDeleteProjectModal();
    }
});
// End of delete project modal


// Project creation form validaiton

function validateProject() {
    let isValid = true;

    const projectName = document.forms["createProject"]["projectName"].value;
    const projectNameError = document.getElementById("project-name-error");
    if (projectName == "") {
        projectNameError.style.display = "block";
        isValid = false;
    } else {
        projectNameError.style.display = "none";
    }

    const clientName = document.forms["createProject"]["clientName"].value;
    const clientNameError = document.getElementById("client-name-error");
    if (clientName == "") {
        clientNameError.style.display = "block";
        isValid = false;
    } else {
    clientNameError.style.display = "none";
    }

    const projectDescription = document.forms["createProject"]["projectDescription"].value;
    const projectDescriptionError = document.getElementById("project-description-error");
    if (projectDescription == "") {
        projectDescriptionError.style.display = "block";
        isValid = false;
    } else {
        projectDescriptionError.style.display = "none";
    }

    const startDate = document.forms["createProject"]["startDate"].value;
    const startDateError = document.getElementById("start-date-error");
    if (startDate == "") {
        startDateError.style.display = "block";
        isValid = false;
    } else {
        startDateError.style.display = "none";
    }

    const endDate = document.forms["createProject"]["endDate"].value;
    const endDateError = document.getElementById("end-date-error");
    if (endDate == "") {
        endDateError.style.display = "block";
        isValid = false;
    } else {
        endDateError.style.display = "none";
    }

    const budget = document.forms["createProject"]["budget"].value;
    const budgetError = document.getElementById("budget-error");
    if (budget == "") {
        budgetError.style.display = "block";
        isValid = false;
    } else {
        budgetError.style.display = "none";
    }

    if (!isValid) {
        event.preventDefault();
    }
}

// end of project creation form validation

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



// User scripts

// Sign in validation

function validateSignIn() {
    let isValid = true;

    const email = document.forms["signIn"]["email"].value;
    const emailError = document.getElementById("email-error");
    if (email == "") {
        emailError.style.display = "block";
        isValid = false;
    } else {
        emailError.style.display = "none";
    }

    const password = document.forms["signIn"]["password"].value;
    const passwordError = document.getElementById("password-error");
    if (password == "") {
        passwordError.style.display = "block";
        isValid = false;
    } else {
        passwordError.style.display = "none";
    }

    if (!isValid) {
        event.preventDefault();
    }

    return isValid;
}

// end of sign in validation

// Sign up validation

function validateSignUp() {
    let isValid = true;

    const firstName = document.forms["signUp"]["firstName"].value;
    const firstNameError = document.getElementById("first-name-error");
    if (firstName == "") {
        firstNameError.style.display = "block";
        isValid = false;
    } else {
        firstNameError.style.display = "none";
    }

    const lastName = document.forms["signUp"]["lastName"].value;
    const lastNameError = document.getElementById("last-name-error");
    if (lastName == "") {
        lastNameError.style.display = "block";
        isValid = false;
    } else {
        lastNameError.style.display = "none";
    }

    const email = document.forms["signUp"]["email"].value;
    const emailError = document.getElementById("email-error");
    if (email == "") {
        emailError.style.display = "block";
        isValid = false;
    } else {
        emailError.style.display = "none";
    }

    const password = document.forms["signUp"]["password"].value;
    const passwordError = document.getElementById("password-error");
    if (password == "") {
        passwordError.style.display = "block";
        isValid = false;
    } else {
        passwordError.style.display = "none";
    }

    const confirmPassword = document.forms["signUp"]["confirmPassword"].value;
    const confirmPasswordError = document.getElementById("confirm-password-error");
    if (confirmPassword == "") {
        confirmPasswordError.style.display = "block";
        isValid = false;
    } else {
        confirmPasswordError.style.display = "none";
    }

    const terms = document.forms["signUp"]["terms"].checked;
    const termsError = document.getElementById("terms-error");
    if (!terms) {
        termsError.style.display = "block";
        isValid = false;
    } else {
        termsError.style.display = "none";
    }

    if (!isValid) {
        event.preventDefault();
    }

    return isValid;
}

// end of sign up validation

// end of user scripts



// file upload scripts

document.addEventListener("DOMContentLoaded", () => {
    fileUpload();
});

function fileUpload() {
    const fileInput = document.querySelector("#file");
    const addImageContainer = document.querySelector(".add-image-container");

    if (!fileInput || !addImageContainer) return;

    addImageContainer.addEventListener("click", () => {
        fileInput.click();
    });

    fileInput.addEventListener("change", (e) => {
        const file = e.target.files[0];
        const circle = document.querySelector("#circle-container");
        const img = document.querySelector("#image-preview");
        const previewContainer = document.querySelector(".image-preview-container");

        if (file) {
            const reader = new FileReader();
            reader.onload = function (event) {
                img.src = event.target.result;
                img.classList.remove("d-none");
                circle.classList.add("d-none");
                previewContainer.classList.remove("d-none");
            };
            reader.readAsDataURL(file);
        } else {
            img.classList.add("d-none");
            circle.classList.remove("d-none");
            previewContainer.classList.add("d-none");
        }
    });
}

// end of file upload scripts


// Notification scripts

document.addEventListener("DOMContentLoaded", () => {
    updateRelativeTimes();
    setInterval(updateRelativeTimes, 60000);
});

function showNotificationModal() {
    const modal = document.querySelector(".notification-container");

    if (modal) {
        if (modal.style.display === "block") {
            modal.style.display = "none";
            document.removeEventListener("click", closeNotificationModal);
        } else {
            modal.style.display = "block";
            document.addEventListener("click", closeNotificationModal);
        }
    }
}

function closeNotificationModal(event) {
    const modal = document.querySelector(".notification-container");
    const bellIcon = document.querySelector(".notification-icon i");

    if (modal && !modal.contains(event.target) && event.target !== bellIcon) {
        modal.style.display = "none";
        document.removeEventListener("click", closeNotificationModal);
    }
}

document.querySelector(".notification-icon i").addEventListener("click", (event) => {
    event.stopPropagation();
    showNotificationModal();
});


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", function (notification) {
    const notificationContainer = document.querySelector('.notification-container-bottom');
    const icon = notification.icon || "/images/default-user.svg";
    const message = notification.message || "No message provided.";
    const created = notification.created ? new Date(notification.created).toISOString() : new Date().toISOString();

    const notificationItem = document.createElement("div");
    notificationItem.className = "notification";
    notificationItem.setAttribute("data-id", notification.id);
    notificationItem.innerHTML = `
        <img class="image" alt="Notification Icon" src="${icon}">
        <div class="notification-info">
            <p class="notification-message">${message}</p>
            <p class="notification-time" data-created="${created}">${formatRelativeTime(created)}</p>
        </div>
        <button class="btn-close" onclick="dismissNotification('${notification.id}')">
            <i class="fa-solid fa-times close"></i>
        </button>
    `;
    notificationContainer.insertBefore(notificationItem, notificationContainer.firstChild);

    updateNotificationCount();
    updateRelativeTimes();
});

connection.on("NotificationDismissed", function (notificationId) {
    const element = document.querySelector(`.notification[data-id="${notificationId}"]`);
    if (element) {
        element.remove();
        updateNotificationCount();
    }
});

connection.start().catch(error => console.error(error))

async function dismissNotification(notificationId) {
    try {
        const res = await fetch(`/api/notifications/dismiss/${encodeURIComponent(notificationId)}`, { method: 'POST' });
        if (res.ok) {
            removeNotification(notificationId)
        } else {
            console.error("Failed to dismiss notification. Status:", res.status);
        }
    } catch (error) {
        console.error("Error dismissing notification:", error);
    }
}


function removeNotification(notificationId) {
    const notification = document.querySelector(`.notification[data-id="${notificationId}"]`);
    if (notification) {
        notification.remove();
        updateNotificationCount();
    }
}

function removeNotification(notificationId) {
    const notification = document.querySelector(`.notification[data-id="${notificationId}"]`);
    if (notification) {
        notification.remove();
        updateNotificationCount()
    }
}


function updateNotificationCount() {
    const notifications = document.querySelector('.notification-container-bottom')
    const notificationNumber = document.querySelector('.notification-number')
    const notificationIcon = document.querySelector('.notification-icon')
    const count = notifications.querySelectorAll('.notification').length

    if (notificationNumber) {
        notificationNumber.textContent = count
    }

    let dot = notificationIcon.querySelector('.notification-indicator')
    if (count > 0 && !dot) {
        dot = document.createElement('span')
        dot.className = 'notification-indicator'
        notificationIcon.appendChild(dot)
    }

    if (count === 0 && dot) {
        dot.remove()
    }
}


function updateRelativeTimes() {
    const notificationItems = document.querySelectorAll('.notification-container-bottom .notification');
    const now = new Date();

    notificationItems.forEach(item => {
        const created = new Date(item.querySelector('.notification-time').getAttribute('data-created'));
        const relativeTime = formatRelativeTime(created);
        item.querySelector('.notification-time').textContent = relativeTime;
    });
}

function formatRelativeTime(created) {
    const now = new Date();
    const difference = now - new Date(created);
    const seconds = Math.floor(difference / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const weeks = Math.floor(days / 7);

    if (minutes < 1) return 'Just now';
    if (minutes < 60) return `${minutes} min ago`;
    if (hours < 24) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    if (days < 7) return `${days} day${days > 1 ? 's' : ''} ago`;
    return `${weeks} week${weeks > 1 ? 's' : ''} ago`;
}
// End of notification scripts