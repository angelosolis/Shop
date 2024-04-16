function onDelete(id) {
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
            $.post("/Shop/ProductDelete",
              {
                  id: id
              },
              function (data, status) {
                  if (data.code == 0) {
                      Swal.fire(data.message, "", "success").then((result) => {
                          if (result.isConfirmed) {
                              window.location = "/Shop/Product";
                          }
                      });
                  }
                  else {
                      Swal.fire(data.message, "", "danger");
                  }
              });
        }
    });
}