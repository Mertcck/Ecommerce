const createToast = (status, message) => {
    const toast = document.createElement('div')
    toast.classList.add("toast")
    toast.setAttribute('role', 'alert')
    toast.innerHTML = `<div class="toast-header">
                            <strong class="me-auto">${status ? "✔ Başarılı" : "❌ Hata" }</strong>
                            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                       </div>
                       <div class="toast-body">${message}</div>`
    return toast
}

const reloadCartItems = (cart) => {
    const items = cart.items.map(i =>`<li>
                            <a class="dropdown-item" href="#">
                                <div class="row">
                                    <div class="col-4">
                                        <img src="${(i.coverImagePath ? i.coverImagePath: "https://placehold.co/400x400?text=Görsel+Yok")}" class="img-fluid" alt="${i.productName}" />
                                    </div>
                                    <div class="col-8">
                                        <span class="h4">${i.productName}</span><br />
                                        <span class="h6 text-muted">${i.quantity} x ₺ ${i.price.toFixed(2) } = ₺ ${i.totalPrice.toFixed(2) }</span>
                                    </div>
                                </div>
                            </a>
                        </li>`)

    return `<a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                    Sepetim (${cart.itemCount})
                </a>
                <ul class="dropdown-menu">
                    ${items.join('\n')}
                    <li><hr class="dropdown-divider"></li>
                    <li>
                        <a class="dropdown-item" href="#">
                            Toplam: ₺ ${cart.cartTotal.toFixed(2)}
                        </a>
                    </li>
                </ul>`
}

const postOptions = {
    method: "POST",
    headers: {
        'content-type': 'application/json'
    }
}

const addToCart = (pid, qty = 1) => {
    fetch(`/api/cart/add/${pid}/${qty}`, postOptions)
        .then(res => res.json())
        .then(data => {
            const toastContainer = document.getElementById('toast-container')
            const cc = document.getElementById('current-cart')
            cc.innerHTML = reloadCartItems(data.currentCart)
            if (toastContainer) {
                const t = createToast(data.result, data.message)
                toastContainer.appendChild(t)
                bootstrap.Toast.getOrCreateInstance(t).show()
            }
        })
        .catch(err => console.error("Sepete Ekleme Hatası:", err))
}

const addToCartButtons = document.querySelectorAll('[id^="btnadd_"]')

for (let btn of addToCartButtons) {
    const pid = btn["id"].split('_')[1]
    btn.addEventListener('click', () => {
        addToCart(pid)
    })
}