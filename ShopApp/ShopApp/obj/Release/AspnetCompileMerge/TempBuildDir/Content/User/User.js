function onAddCart(prodId) {
    // Using Javascript
    var qty = document.getElementById("txtQty").value;

    Swal.fire({
        title: "Are you sure?",
        icon: "question",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        showCancelButton: true,
        confirmButtonText: "OK",
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            // save here
            $.post("/Home/AddCart",
              {
                  prodId: prodId,
                  qty: qty
              },
              function (data, status) {
                  if (data.code == 0) {
                      Swal.fire(data.message, "", "success");
                      loadCartCount();
                  }
                  else {
                      Swal.fire(data.message, "", "danger");
                  }
              });
        }
    });
}