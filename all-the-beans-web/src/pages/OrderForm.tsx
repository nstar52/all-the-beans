import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getAllBeans } from '../services/api'
import type { Bean } from '../types/Bean'

function OrderForm() {
  const { beanId } = useParams()
  const navigate = useNavigate()
  const [beans, setBeans] = useState<Bean[]>([])
  const [selectedBean, setSelectedBean] = useState<Bean | null>(null)
  const [quantity, setQuantity] = useState(1)
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [address, setAddress] = useState('')
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [submitted, setSubmitted] = useState(false)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadBeans()
  }, [])

  useEffect(() => {
    if (beanId && beans.length > 0) {
      const bean = beans.find(b => b.id === parseInt(beanId))
      if (bean) {
        setSelectedBean(bean)
      }
    }
  }, [beanId, beans])

  const loadBeans = async () => {
    try {
      const data = await getAllBeans()
      setBeans(data)
      setLoading(false)
    } catch (error) {
      console.error('Failed to load beans', error)
      setLoading(false)
    }
  }

  const validate = () => {
    const newErrors: Record<string, string> = {}

    if (!name.trim()) {
      newErrors.name = 'Name is required'
    }

    if (!email.trim()) {
      newErrors.email = 'Email is required'
    } else if (!email.includes('@')) {
      newErrors.email = 'Please enter a valid email'
    }

    if (!address.trim()) {
      newErrors.address = 'Address is required'
    }

    if (!selectedBean) {
      newErrors.bean = 'Please select a bean'
    }

    if (quantity < 1) {
      newErrors.quantity = 'Quantity must be at least 1'
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    if (!validate()) {
      return
    }

    if (!selectedBean) return

    const order = {
      name,
      email,
      address,
      beanId: selectedBean.id,
      beanName: selectedBean.name,
      quantity,
      totalPrice: (selectedBean.cost * quantity).toFixed(2),
      date: new Date().toISOString()
    }

    const orders = JSON.parse(localStorage.getItem('orders') || '[]')
    orders.push(order)
    localStorage.setItem('orders', JSON.stringify(orders))

    setSubmitted(true)
  }

  const totalPrice = selectedBean ? (selectedBean.cost * quantity).toFixed(2) : '0.00'

  if (loading) {
    return <div className="text-center mt-5">Loading...</div>
  }

  if (submitted) {
    return (
      <div className="text-center mt-5">
        <div className="alert alert-success" role="alert">
          <h4>Order Submitted Successfully!</h4>
          <p>Thank you for your order. We'll process it shortly.</p>
        </div>
        <button className="btn btn-primary" onClick={() => navigate('/beans')}>
          Back to Beans
        </button>
      </div>
    )
  }

  return (
    <div className="row justify-content-center">
      <div className="col-md-8">
        <h2 className="mb-4">Place Your Order</h2>
        
        <div className="card shadow-sm mb-4">
          <div className="card-body">

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="name" className="form-label">Name *</label>
            <input
              type="text"
              className={`form-control ${errors.name ? 'is-invalid' : ''}`}
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            {errors.name && <div className="invalid-feedback">{errors.name}</div>}
          </div>

          <div className="mb-3">
            <label htmlFor="email" className="form-label">Email *</label>
            <input
              type="email"
              className={`form-control ${errors.email ? 'is-invalid' : ''}`}
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            {errors.email && <div className="invalid-feedback">{errors.email}</div>}
          </div>

          <div className="mb-3">
            <label htmlFor="address" className="form-label">Address *</label>
            <textarea
              className={`form-control ${errors.address ? 'is-invalid' : ''}`}
              id="address"
              rows={3}
              value={address}
              onChange={(e) => setAddress(e.target.value)}
            />
            {errors.address && <div className="invalid-feedback">{errors.address}</div>}
          </div>

          <div className="mb-3">
            <label htmlFor="bean" className="form-label">Select Bean *</label>
            <select
              className={`form-select ${errors.bean ? 'is-invalid' : ''}`}
              id="bean"
              value={selectedBean?.id || ''}
              onChange={(e) => {
                const bean = beans.find(b => b.id === parseInt(e.target.value))
                setSelectedBean(bean || null)
              }}
            >
              <option value="">Choose a bean...</option>
              {beans.map(bean => (
                <option key={bean.id} value={bean.id}>
                  {bean.name} - £{bean.cost.toFixed(2)}
                </option>
              ))}
            </select>
            {errors.bean && <div className="invalid-feedback">{errors.bean}</div>}
          </div>

          <div className="mb-3">
            <label htmlFor="quantity" className="form-label">Quantity *</label>
            <input
              type="number"
              className={`form-control ${errors.quantity ? 'is-invalid' : ''}`}
              id="quantity"
              min="1"
              value={quantity}
              onChange={(e) => setQuantity(parseInt(e.target.value) || 1)}
            />
            {errors.quantity && <div className="invalid-feedback">{errors.quantity}</div>}
          </div>

          {selectedBean && (
            <div className="mb-3">
              <h5>Total: £{totalPrice}</h5>
            </div>
          )}

          <div className="d-flex gap-2 mt-4">
            <button type="submit" className="btn btn-primary">
              Submit Order
            </button>
            <button type="button" className="btn btn-outline-secondary" onClick={() => navigate('/beans')}>
              Cancel
            </button>
          </div>
        </form>
          </div>
        </div>
      </div>
    </div>
  )
}

export default OrderForm

