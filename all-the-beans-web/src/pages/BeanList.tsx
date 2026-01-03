import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { getAllBeans } from '../services/api'
import type { Bean } from '../types/Bean'

function BeanList() {
  const [beans, setBeans] = useState<Bean[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  useEffect(() => {
    loadBeans()
  }, [])

  const loadBeans = async () => {
    try {
      const data = await getAllBeans()
      setBeans(data)
      setError(null)
    } catch (error) {
      console.error('Failed to load beans', error)
      if (error instanceof Error) {
        setError(`Failed to load beans: ${error.message}`)
      } else {
        setError('Failed to load beans. Make sure the API is running.')
      }
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="text-center mt-5">Loading...</div>
  }

  if (error) {
    return (
      <div className="alert alert-danger mt-4" role="alert">
        {error}
      </div>
    )
  }

  if (beans.length === 0) {
    return (
      <div>
        <h2 className="mb-4">All Beans</h2>
        <p>No beans available.</p>
      </div>
    )
  }

  return (
    <div>
      <h2 className="mb-4">All Beans</h2>
      <div className="row">
        {beans.map((bean) => (
          <div key={bean.id} className="col-md-4 mb-4">
            <div 
              className="card h-100" 
              style={{ cursor: 'pointer' }}
              onClick={() => navigate(`/beans/${bean.id}`)}
            >
              {bean.image && (
                <img 
                  src={bean.image} 
                  className="card-img-top" 
                  alt={bean.name}
                  style={{ height: '200px', objectFit: 'cover' }}
                />
              )}
              <div className="card-body">
                <h5 className="card-title">{bean.name}</h5>
                <p className="card-text">
                  <small className="text-muted">{bean.country}</small>
                </p>
                <p className="card-text">£{bean.cost.toFixed(2)}</p>
                {bean.isBeanOfTheDay && (
                  <span className="badge bg-warning text-dark">Bean of the Day</span>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default BeanList

