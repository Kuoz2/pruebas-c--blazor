let dataTableInstance;

window.iniciarDataTblae = function (element) {
    let dataTable = $('#dataTable');
    if ($.fn.DataTable.isDataTable('#dataTable')) {
        $('#dataTable').DataTable().clear();
        $('#dataTable').DataTable().destroy();
        $('#dataTable').empty();
    } // Limpiar el DataTable existente si lo hay
    $.fn.dataTable.ext.search = [];


    // Inicializa el DataTable solo si el elemento dataTable est� presente
    if (dataTable.length > 0) {
        dataTableInstance = dataTable.DataTable({
            "lengthMenu": [[8, 25, 50, -1], [8, 25, 50, "Todos"]],
            "defaultContent": "<button>Editar</button>",
            "language": {
                "lengthMenu": "Mostrar _MENU_ registros por p�gina",
                "zeroRecords": "No se encontraron registros",
                "info": "",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtrado de _MAX_ registros totales)",
                "search": "Buscar Por c�digo:",
                "paginate": {
                    "first": "Primero",
                    "last": "�ltimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                }
            }
        });
    }
}

window.cleanDataTable = function () {
    if ($.fn.DataTable.isDataTable('#dataTable')) {
        $('#dataTable').DataTable().clear();
        $('#dataTable').DataTable().destroy();
        $('#dataTable').empty();
    }
}

window.destroyDataTable = function () {
}

//Crear ventas
var datosIngresados = [];
var numerodeFila = 0;
var TotalObtenido = 0;
var descontar = 0

window.CrearVenta =  function (tableId, inputId, columnIndex) {
   
    if (tableId.id == "ventaTable") {
        if ($.fn.DataTable.isDataTable('#ventaTable')) {
            $('#ventaTable').DataTable().clear();
            $('#ventaTable').DataTable().destroy();
            $('#ventaTable').empty();
        }

        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var searchValue = $('input[id^="dt-search-"]').val() ? $('input[id^="dt-search-"]').val().trim() : ''; // Comprobaci�n adicional
                var codigo = data[columnIndex]; // Supongamos que la columna del c�digo es la primera (�ndice 0)
                return codigo === searchValue;
            }
        );

        var table = $('#ventaTable').DataTable({
            "paging": false,
            "lengthMenu": [[], []],
            "defaultContent": "<button>Editar</button>",
            "language": {
                "lengthMenu": "",
                "zeroRecords": "No se encontraron registros",
                "info": "",
                "infoEmpty": "",
                "infoFiltered": "",
                "search": "Buscar Por c�digo:",
                "paginate": {
                    "first": "",
                    "last": "",
                    "next": "",
                    "previous": ""
                }
            }
        });

        $('input[id^="dt-search-"]').on('keyup input', function () {
            table.draw(); // Vuelve a dibujar la tabla con el filtro aplicado
        });

        $('input[id^="dt-search-"]').on('input', function () {
            if ($.fn.DataTable.isDataTable('#ventaTable')) {
                var texto = $(this).val().trim();
                console.log('Texto ingresado:', texto);
                table.search(texto).draw();
            } else {
                console.log('Error: DataTables no est� inicializado correctamente.');
            }

            const inputProducto = $('input[id^="dt-search-"]');
            let temporizador;

            if (inputProducto) {
                inputProducto.on('input', function () {
                    clearTimeout(temporizador);
                    const $this = $(this);
                    let texto = $this.val().trim();
                    if (texto == '') {
                        return;
                    }

                    function buscarProducto() {
                        const tabla = $('#ventaTable').DataTable();
                        const fila = tabla.rows().nodes().toArray().find(row => {
                            const datosFila = tabla.row(row).data(); // Obtener los datos de la fila usando DataTables
                            if (datosFila[0].indexOf(texto) > -1 && datosFila[0] === texto) { return datosFila[0] === texto; }
                        });
                        if (fila) {
                            const datosFila = tabla.row(fila).data(); // Obtener los datos de la fila usando DataTables
                            const pcodigo = datosFila[0]; // Primer columna
                            const nombre = datosFila[1]; // Segunda columna
                            const precio = datosFila[2]; // Tercera columna
                            const cantidad = datosFila[3]; // Cuarta columna
                            const id = datosFila[4]; // Quinta columna
                            console.log('Cantidad encontrada:', pcodigo, nombre, precio, cantidad, id);

                            if (cantidad === '0') {
                                console.log("La cantidad est� en 0, no se puede agregar este producto");
                                return;
                            }
                            datosIngresados.push({ Pcodigo: pcodigo, Nombre: nombre, Precio: precio, Cantidad: cantidad, Id: id });
                            agregar(pcodigo, nombre, precio, cantidad, id);
                            $this.val('');
                            return
                        }
                    }
                        buscarProducto()
                     
                });
            } else {
                console.log('Error: No se encontr� el input de b�squeda.');
            }
        });

        function agregar(pcodigo, nombre, precio, cantidad, id) {
            console.log("Se agrega el producto al carrito");
            const carritoElement = $('#TableVentas');
            const totalElement = $('#Total');

            let existeCarrito = carritoElement.find('table');

            if (existeCarrito.length === 0) {
                const tabla = $('<table>').addClass('table table-striped');
                const thead = $('<thead>');
                const headRow = $('<tr>');
                const headers = ['C�digo', 'Nombre', 'Precio', 'Cantidad', 'Acci�n'];
                headers.forEach(headerText => {
                    const th = $('<th>').text(headerText);
                    headRow.append(th);
                });
                thead.append(headRow);
                tabla.append(thead);
                carritoElement.append(tabla);
                existeCarrito = tabla;
            }

            let tbody = existeCarrito.find('tbody');
            if (tbody.length === 0) {
                tbody = $('<tbody>');
                existeCarrito.append(tbody);
            }

            const row = $('<tr>');
            const pcodigoCell = $('<td>').text(pcodigo);
            const nombreCell = $('<td>').text(nombre);
            const precioCell = $('<td>').text(precio);
            const cantidadCell = $('<td>').text(cantidad);
            const eliminarCell = $('<td>');
            const eliminarBtn = $('<button>').text('Eliminar').addClass('btn btn-danger');

            eliminarBtn.on('click', function () {
                const filaActual = $(this).closest('tr');
                const precio = parseInt(filaActual.find('td:nth-child(3)').text());
                const cantidad = parseInt(filaActual.find('td:nth-child(4)').text());
                const indiceFila = obtenerIndiceFila(filaActual);

                console.log('La fila actual est� en la posici�n:', indiceFila);
                datosIngresados.splice(indiceFila, 1);
                let totalActual = parseInt(totalElement.text().replace(/\$/g, '').replace(/^Total:\s*/, ''));
                totalActual -= precio;
                totalElement.text('Total: $' + totalActual);
                filaActual.remove();
                return totalActual;
            });

            function obtenerIndiceFila(fila) {
                const tbody = fila.closest('tbody');
                const filas = tbody.find('tr').toArray();
                let indice = 0;
                for (const filaIterada of filas) {
                    if (filaIterada === fila[0]) {
                        break;
                    }
                    indice++;
                }
                return indice !== -1 ? indice : null;
            }

            eliminarCell.append(eliminarBtn);
            row.append(pcodigoCell, nombreCell, precioCell, cantidadCell, eliminarCell);
            tbody.append(row);
            mostrarTotal(carritoElement);

            function mostrarTotal(element) {
                let total = 0;
                const filas = element.find('tbody tr');
                filas.each(function () {
                    const fila = $(this);
                    const precio = parseInt(fila.find('td:nth-child(3)').text());
                    total += precio;
                });
                totalElement.text('Total: $' + total);
            }

            console.log('Productos en el carrito:', datosIngresados);
        }
    }

        btnGuardarVenta = document.getElementById('guardarVenta');
    btnGuardarVenta.addEventListener('click', function () {
        const countById = {};
        const sumaPreciosById = {};
        let obtenercantidad = 0;
        if (datosIngresados.length !== 0) {
            datosIngresados.forEach((res) => {
                console.log('res del button de guardado', res)
                const cantidad = res.Id;
                countById[cantidad] = (countById[cantidad] || 0) + 1;
                obtenercantidad = res.cantidad
            });
            datosIngresados.forEach((res, index) => {
                const repeticiones = countById[res.id]
                datosIngresados[index].cantidad == repeticiones;
            })
            const datosActualizados = {
                datosIngresados: datosIngresados
            }
            $.ajax({
                url: "",
                method: "PUT",
                type: "PUT",
                headers: {

                },
                data: JSON.stringify(datosActualizados),
                contentType: "application/json",
                dataType: "json",
                success: function (response) {
                    if (response.success == 'informacion guardada') {
                        location.reload();
                    } else { console.log('error ', response.error) }
                },
                error: function (xhr, errmsg, err) {
                    console.log('status del guardado', xhr.status + '' + xhr.respnseText)
                }
            });
            const productoVendido = "";
            datosIngresados.forEach(producto => {
                const id = producto.id
                const precio = parseInt(producto.precio);
                sumarPrecioById[id] = (sumarPrecioById[id] || 0) + precio;
                productoVendido = producto.Nombre
                });
        }
        object.key(countById).forEach(async id => {
            const fechaActual = new Date();
            const a�o = fechaActual.getFullYear();
            const mes = fechaActual.getMonth() + 1;
            const dia = fechaActual.getDay();
            const hoy + '/' + mes + '/' + a�o;
            let cantidad = countById[id];
            let preciov = sumaPreciosById[id];
            let fechav = hoy.toString();
            let metov = document.getElementById('metodoPago').value;
            let productoin = 0;
            const productosJSON = [];
            const Productos = {
                Preciov: preciov,
                Cantidadv: cantidad,
                Fechav: fechav,
                Metodov: metov,
                Productoin: productoVendido,
                Productoid:id
                
            }
        })
        })
}

function mostrarTotal(carritoElement) {
    // Eliminar cualquier fila existente que muestre el total
    this.descontar = 0
    const totalElement = document.getElementById('Total');
    if (totalElement) {
        totalElement.remove();
    }

    // Calcular el total sumando los precios de todos los productos

    carritoElement.querySelectorAll('table tbody tr').forEach(row => {
        const precio = parseInt(row.querySelector('td:nth-child(3)').textContent);
        this.descontar += precio;
    });

    // Crear un elemento div para mostrar el total
    const totalDiv = document.createElement('div');
    totalDiv.id = 'Total';
    totalDiv.textContent = 'Total: $' + this.descontar; // Mostrar el total con dos decimales

    // Agregar el total al contenedor carritoElement
    carritoElement.appendChild(totalDiv);
    return this.descontar
}