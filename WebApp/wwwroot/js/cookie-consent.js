document.addEventListener("DOMContentLoaded", function () {
    if (!getCookie("cookieConsent")) {
        showCookieModal()
    }
})

function showCookieModal() {
    const modal = document.getElementById("cookie-modal")
    if (modal) {
        modal.style.display = "flex"
    }

    const consentValue = getCookie("cookieConsent")
    if (!consentValue) {
        return
    }

    try {
        const consent = JSON.parse(consentValue)
        document.getElementById("cookie-functional").checked = consent.functional
        document.getElementById("cookie-analytics").checked = consent.analytics
        document.getElementById("cookie-marketing").checked = consent.marketing
    }
    catch (error) {
        console.error("Error when handling cookie consent: ", error)
    }
}

function hideCookieModal() {
    const modal = document.getElementById("cookie-modal")
    if (modal) {
        modal.style.display = "none"
    }
}

function getCookie(name) {
    const nameEQ = name + "="
    const cookies = document.cookie.split(';')
    for (let cookie of cookies) {
        cookie = cookie.trim()
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
     return null
}

function setCookie(name, value, days) {
    let expires = ""
    if (days) {
        const date = new Date()
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000))
        expires = "; expires=" + date.toUTCString()
    }

    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodedValue}${expires}; path=/; SameSite=Lax`
}

async function acceptAll() {
    const consent = {
        essential: true,
        functional: true,
        analytics: true,
        marketing: true
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await handleConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById("cookie-consent-form");
    const formData = new FormData(form);

    const consent = {
        essential: true,
        functional: formData.get("functional") === "on",
        analytics: formData.get("analytics") === "on",
        marketing: formData.get("marketing") === "on"
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await handleConsent(consent)
    hideCookieModal()
}

async function handleConsent(consent) {
    try {
        const response = await fetch("/cookies/setcookies", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(consent)
        })

        if (!response.ok) {
            throw new Error("Failed to set cookies", await result.text())
        }
    }
    catch (error) {
        console.error("Error:: ", error)
    }
}