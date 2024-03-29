const body = document.querySelector("body"),
    sidebar = document.querySelector(".sidebar"),
    toggle = document.querySelector('.toggle'),
    searchBtn = body.querySelector('.search-box'),
    modeSwitch = body.querySelector('.toggle-switch'),
    modeText = body.querySelector('.mode-text');

toggle.addEventListener("click", () =>
{
    sidebar.classList.toggle("close");
})

searchBtn.addEventListener("click", () =>
{
    sidebar.classList.remove("close");
})

modeSwitch.addEventListener("click", () =>
{
    body.classList.toggle("dark");
    
    if(body.classList.contains("dark"))
    {
        modeText.innerText = "День";
    }
    else
    {
        modeText.innerText = "Ночь";
    }
})