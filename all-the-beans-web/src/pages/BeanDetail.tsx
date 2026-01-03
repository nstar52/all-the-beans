import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getBeanById } from '../services/api'
import type { Bean } from '../types/Bean'

function BeanDetail() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [bean, setBean] = useState<Bean | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (id) {
      loadBean(parseInt(id))
    }
  }, [id])

  const loadBean = async (beanId: number) => {
    try {
      const data = await getBeanById(beanId)
      setBean(data)
      setError(null)
    } catch (error) {
      console.error('Failed to load bean', error)
      setError('Failed to load bean details.')
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="text-center mt-5">Loading...</div>
  }

  if (error || !bean) {
    return (
      <div>
        <div className="alert alert-danger mt-4" role="alert">
          {error || 'Bean not found'}
        </div>
        <button className="btn btn-secondary" onClick={() => navigate('/beans')}>
          Back to Beans
        </button>
      </div>
    )
  }

  return (
    <div>
      <button 
        className="btn btn-outline-secondary mb-3" 
        onClick={() => navigate('/beans')}
      >
        ← Back to Beans
      </button>
      
      <div className="row">
        <div className="col-md-6">
          {bean.image && (
            <img 
              src={bean.image} 
              className="img-fluid rounded" 
              alt={bean.name}
            />
          )}
        </div>
        <div className="col-md-6">
          <h2>{bean.name}</h2>
          {bean.isBeanOfTheDay && (
            <span className="badge bg-warning text-dark mb-2">Bean of the Day</span>
          )}
          <p className="text-muted">{bean.country}</p>
          <p><strong>Colour:</strong> {bean.colour}</p>
          <p><strong>Price:</strong> £{bean.cost.toFixed(2)}</p>
          <p>{bean.description}</p>
        </div>
      </div>
    </div>
  )
}

export default BeanDetail

