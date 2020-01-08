function ajax (options) {
	try {
		let xhr = new XMLHttpRequest()

		if (typeof options.callback === 'function') {
			xhr.addEventListener('load', function (event) {
				options.callback(event.currentTarget)
			}, false)
		}

		xhr.open(options.method, options.baseUrl + options.url)

		xhr.setRequestHeader('Content-Type', 'application/json')
		if (options.headers) {
			options.headers.forEach(function (header) {
				xhr.setRequestHeader(header.name, header.value)
			})
		}

		if (options.payload) {
			xhr.send(options.payload)
		} else {
			xhr.send()
		}
	} catch (error) {
		console.error(error.Message)
	}
}

export default {
	ajax: function (options) {
		options.baseUrl = 'https://localhost:5001'
		ajax(options)
	}
}
