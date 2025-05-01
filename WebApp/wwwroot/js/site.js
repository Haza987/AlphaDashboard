// Member scripts

// Member more modal

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

    const phone = document.forms["createMember"]["phone"].value;
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

// End of project more modal


// Project info modal

async function showProjectInfoModal(projectId) {
    const modal = document.getElementById("project-info-modal");

    if (modal) {
        const response = await fetch(`/Projects/UpdateProject?id=${projectId}`);
        if (response.ok) {
            const project = await response.json();
            modal.querySelector("#project-id-custom").textContent = project.projectId;
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
        event.preventDefault(); // Prevent default form submission

        const form = event.target;
        const formData = new FormData(form);
        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
            });

            if (response.ok) {
                location.reload(); // Reload the page on success
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

        // Ensure at least one member is selected
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

// End of add project modal


// Shared selected members container

const selectedMembers = new Set();
function updateSelectedMembers(dropdownId, containerId) {
    const dropdown = document.getElementById(dropdownId);
    const selectedMemberId = dropdown.value;
    const selectedMemberName = dropdown.options[dropdown.selectedIndex]?.getAttribute('data-name');

    // Ignore the default option
    if (selectedMemberId === "-- Assign members --" || !selectedMemberName) {
        console.error("[ERROR] Member ID or Name is invalid.");
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
        console.log(`Member added: ID=${selectedMemberId}, Name=${selectedMemberName}`);
    }
}

//add this back in when image functionality works
//<img class="member-icon" src="" alt="Member Image" />


function removeSelectedMember(memberId, containerId) {
    // Remove the member from the selectedMembers set
    selectedMembers.delete(memberId);

    // Find the container where the member div is located
    const container = document.getElementById(containerId);

    // Find the specific member div within the container
    const memberDiv = container.querySelector(`.selected-member[data-id="${memberId}"]`);

    // Remove the member div if it exists
    if (memberDiv) {
        memberDiv.remove();
        console.log(`Member removed: ID=${memberId}`);
    } else {
        console.error(`Member div not found for ID=${memberId}`);
    }

    // Remove the hidden input field for the member
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
        } catch (error) {
            console.error("[DEBUG] Error while updating project:", error);
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